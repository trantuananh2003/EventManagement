
using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        Task<OrderHeader> UpdateAsync(OrderHeader entity);
    }
}
