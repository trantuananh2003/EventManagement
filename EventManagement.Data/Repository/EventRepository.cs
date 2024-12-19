using Dapper;
using EventManagement.Data.Dapper;
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{

    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IDapperHelper _dapper;

        public EventRepository(ApplicationDbContext db, IDapperHelper dapper) : base(db) {
            _db = db;
            _dapper = dapper;
        }

        public void Update(Event entity)
        {
            _db.Events.Update(entity);
        }

        public async Task<(IEnumerable<T>, int)> GetEventsForOrganization<T>(string idOrganization, string searchString, bool isUpComing ,string status, int pageSize, int pageIndex)
        {
            if(pageSize == 0)
                pageSize = 10;


            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("idOrganization", idOrganization, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("status", status, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("searchString", searchString, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("isUpComing", isUpComing, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);
            parameters.Add("pageSize", pageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("pageIndex", pageIndex, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("totalRecords", 0, System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var result = await _dapper.ExecuteStoreProcedureReturnList<T>("spGetAllEventForOrganization", parameters);
            var totalRecord = parameters.Get<int>("totalRecords");

            return (result, totalRecord);
        }
    }
}
