using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries.ModelDto
{
    public class EventPaymentSummary
    {
        public string IdEvent { get; set; }
        public string NameEvent { get; set; }
        public string UrlImage { get; set; }
        public string OrganizationName { get; set; }
        public int TotalPayment { get; set; } // Tổng số tiền
    }

}
