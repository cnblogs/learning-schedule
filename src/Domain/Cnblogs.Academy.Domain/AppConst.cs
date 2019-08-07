using System;

namespace Cnblogs.Academy.Domain
{
    public class AppConst
    {
        public static string AppId { get; set; }

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
