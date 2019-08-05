using System;

namespace Cnblogs.Academy.Domain
{
    public class AppConst
    {
        public static string AppId { get; set; }
        public const string ProviderTenancyOwner = "Academy.CourseMall.Tennacy.Owner";

        public const string ProviderTenancyName = "Academy.CourseMall.Tenancy";

        private static string _domainAddress;
        public static string DomainAddress
        {
            get
            {
                if (String.IsNullOrEmpty(_domainAddress))
                {
                    return "https://academy.cnblogs.com";
                }
                else
                {
                    return _domainAddress;
                }
            }
            set
            {
                _domainAddress = value;
            }
        }
    }
}
