
using EventManagement.Data.Helpers;
using EventManagement.Data.Models;
using EventManagement.Data.Queries.ModelDto;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
        Task<PagedList<UserOrderOverviewDto>> GetUserOrders(string IdUser, string searchString,
            string statusFilter, int pageSize, int pageNumber);
        Task<(IEnumerable<AdminOrderOverviewDto>, int)> GetAdminOrders(string IdOrganizatoin, string search, int pageSize, int pageNumber );
        Task UpdateStatusOrderHeader(string orderHeaderId,string stripePaymentIntentId, string status);
    }
}
