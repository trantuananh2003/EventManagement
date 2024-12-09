using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Common
{
    public static class SD_Role_Permission
    {
        //Organization
        public const string Organization_ClaimType = "Organization";

        public const string AddEvent_ClaimValue = "AddEvent";
        public const string UpdateEvent_ClaimValue = "UpdateEvent";
        public const string ReportEvent_ClaimValue = "ViewReportEvent";
        public const string SupportChat_ClaimValue = "SupportChat";
        public const string EditPurchasedTicket_ClaimValue = "EditPurchasedTicket";
        public const string ManageMember_ClaimValue = "ManageMember";
        public const string ManageRole_ClaimValue = "ManageRole";

        //Admin
        public const string Admin_ClaimType = "Admin";
        public const string Admin_ClaimValue = "All";
    }
}
