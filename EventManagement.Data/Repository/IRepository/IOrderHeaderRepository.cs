
using EventManagement.Data.Models;
using EventManagement.Data.Models.ModelDto;
using EventManagement.Data.Queries.ModelDto;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
        IQueryable<UserOrderOverviewDto> GetUserOrders(string IdUser);
        Task<(IEnumerable<AdminOrderOverviewDto>, int)> GetAdminOrders(string IdOrganizatoin, string search, int pageSize, int pageNumber );
    }
}
