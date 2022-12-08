using Xunit;

namespace Parlis.Server.UnitTests
{
    public class MatchManagementTest
    {
        private readonly BusinessLogic.Service service = new BusinessLogic.Service();
        private readonly int code = 123456;

        [Fact]
        public void CheckMatchExistenceFailedTest()
        {
            bool isCreated = service.CheckMatchExistence(code);
            Assert.False(isCreated);
        }

        [Fact]
        public void CheckMatchExistenceSuccessTest()
        {
            service.CreateMatch(code);
            bool isCreated = service.CheckMatchExistence(code);
            Assert.True(isCreated);
        }
    }
}