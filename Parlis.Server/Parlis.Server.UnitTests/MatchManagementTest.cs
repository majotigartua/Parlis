using Xunit;

namespace Parlis.Server.UnitTests
{
    public class MatchManagementTest
    {
        [Fact]
        public void CheckMatchExistenceTest()
        {
            int code = 123456;
            BusinessLogic.Service service = new BusinessLogic.Service();
            service.CreateMatch(code);
            bool isCreated = service.CheckMatchExistence(code);
            Assert.True(isCreated);
        }
    }
}