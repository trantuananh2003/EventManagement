using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Common
{
    //Static Define
    public static class SD
    {
        public const string Role_Customer = "customer";

        public const string Status_OnSale = "onsale";
        public const string Status_SoldOut = "soldout";
        public const string Status_Cancelled = "cancelled";
        public const string Status_Postponed = "postponed";

        public const string Privacy_Public = "Public";
        public const string Privacy_Private = "Private";

        public const string EventType_Single = "single";
        public const string EventType_Multiple = "multiple";

        public const string SD_Storage_Containter = "eventmanagement";

        public const string SD_URL_LINK_RESETPASSWORD = "http://localhost:3000/reset-password";
    }

    public enum Privacy
    {
        Public,  // Thay thế 'public' bằng 'Public'
        Private  // Thay thế 'private' bằng 'Private'
    }

    public enum EOrderCreate
    {
        OutOfStock,
        NotFoundItem,
        Done,
    }


}
