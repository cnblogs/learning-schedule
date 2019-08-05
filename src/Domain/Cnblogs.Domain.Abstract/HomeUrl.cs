namespace Cnblogs.Domain.Abstract
{
    public class HomeUrl
    {
        public const string Prefix = "//home.cnblogs.com/u/";

        public static string Of(string alias)
        {
            return Prefix + alias;
        }
    }
}
