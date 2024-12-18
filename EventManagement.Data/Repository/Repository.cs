﻿using EventManagement.Data.DataConnect;
using EventManagement.Data.Helpers;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EventManagement.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = false, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (!tracked)
                {
                    query = query.AsNoTracking();
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the entity.", ex);
            }
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all the entities.", ex);
            }
        }

        public async Task<PagedList<T>> GetPagedAllAsync(
            Expression<Func<T, bool>>? filter = null
            , List<(Expression<Func<T, object>> orderBy, bool isDescending)>? orderByList = null
            , string? includeProperties = null
            , int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                // Sắp xếp theo nhiều tiêu chí nếu có
                if (orderByList != null && orderByList.Any())
                {
                    // Áp dụng sắp xếp đầu tiên
                    var firstOrderBy = orderByList.First();
                    IOrderedQueryable<T> orderedQuery = firstOrderBy.isDescending
                        ? query.OrderByDescending(firstOrderBy.orderBy)
                        : query.OrderBy(firstOrderBy.orderBy);

                    // Áp dụng các sắp xếp tiếp theo
                    foreach (var order in orderByList.Skip(1))
                    {
                        orderedQuery = order.isDescending
                            ? orderedQuery.ThenByDescending(order.orderBy)
                            : orderedQuery.ThenBy(order.orderBy);
                    }

                    // Cập nhật lại truy vấn với kết quả đã sắp xếp
                    query = orderedQuery;
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return await PagedList<T>.ToPagedList(query, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the all paged entities.", ex);
            }
        }

        public async Task CreateAsync(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the entity.", ex);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the entity.", ex);
            }
        }

        public void RemoveRange(List<T> listEntity)
        {
            try
            {
                dbSet.RemoveRange(listEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the list entity.", ex);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the changes.", ex);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }
    }
}
