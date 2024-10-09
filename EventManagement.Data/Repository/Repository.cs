using EventManagement.Data.DataConnect;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventManagement.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(string includeProperties)
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }

}
