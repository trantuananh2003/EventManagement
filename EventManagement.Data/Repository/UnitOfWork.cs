using EventManagement.Data.Dapper;
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _dbContext;
        private IDapperHelper _dapper;
        private ITicketRepository  _ticketRepository;
        private IOrderHeaderRepository _orderHeaderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IPurchasedTicketRepository _purchasedTicketRepository;
        private IEventRepository _eventRepository;
        private IEventDateRepository _eventDateRepository;

        public UnitOfWork(ApplicationDbContext dbContext, IDapperHelper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        public ITicketRepository TicketRepository => _ticketRepository ??= new TicketRepository(_dbContext);
        public IOrderHeaderRepository OrderHeaderRepository => _orderHeaderRepository ??= new OrderHeaderRepository(_dbContext);
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository ??= new OrderDetailRepository(_dbContext);
        public IPurchasedTicketRepository PurchasedTicketRepository => _purchasedTicketRepository ??= new PurchasedTicketRepository(_dbContext);
        public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_dbContext, _dapper);
        public IEventDateRepository EventDateRepository => _eventDateRepository ??= new EventDateRepository(_dbContext);

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        //Dispose connect of Database
        public void Dispose() 
        {
            if (_dbContext != null) _dbContext.Dispose();
        }
    }
}
