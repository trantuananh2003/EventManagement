
using EventManagement.Data.Models;
using EventManagement.Data.Queries.ModelDto;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
        IQueryable<DataOverviewOrderDto> GetListOverviewOrderDtos(string IdUser);
    }
}
