using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Common
{
    public static class SD_Role_Permission
    {
        #region Role for Organization
        public const string Organization_ClaimType = "Organization";
        //Event
        public const string AddEvent_ClaimValue = "AddEvent";
        public const string UpdateEvent_ClaimValue = "UpdateEvent";
        public const string ViewReportEvent_ClaimValue = "ViewReportEvent";

        //Support chat
        public const string SupportChat_ClaimValue = "SupportChat";

        //OverView order
        public const string ManageOrderOverView = "OrderOverView";

        //Organization - Team manage
        public const string ManageOrganization_ClaimValue = "ManageOrganization";
        public const string ManageMember_ClaimValue = "ManageMember";
        public const string ManageRole_ClaimValue = "ManageRole";
        #endregion

        #region Admin
        public const string Admin_ClaimType = "Admin";
        public const string Admin = "All";
        #endregion
    }
}
