using Cnblogs.Academy.WebAPI.Setup;
using Xunit;

namespace AcademyUnitTest
{
    public class RetrieveHostUrlTest
    {
        [Fact]
        public void TestName()
        {
            var result = UrlUtility.RetrieveHostUrl("http://*:80");
            Assert.Equal("http://localhost:80", result);

            result = UrlUtility.RetrieveHostUrl("http://+:80");
            Assert.Equal("http://localhost:80", result);
        }
    }
}
