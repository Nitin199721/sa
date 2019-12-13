using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Repository
{
    public static class ApplicationConstants
    {
        public const string ServiceAccountUser = "NicoFinanceUser";

        public const string ServiceAccountAd = "NICOAD";

        public const string ServiceAccountUserPassword = "n1c0F!nanc3U$er";

        public static Impersonator GetImpersonatedUser()
        {
            return new Impersonator(ServiceAccountUser, ServiceAccountAd, ServiceAccountUserPassword);
        }
    }
}
