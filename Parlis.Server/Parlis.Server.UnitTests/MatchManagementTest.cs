using Parlis.Server.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class MatchManagementTest
    {
        private int code = 901182;
        PlayerProfile playerProfile = new PlayerProfile
        {
            Username = "testguy"
        };

        [Fact]
        public void CheckMatchExistenceHappyPathTest()
        {
            bool isCreated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            service.CreateMatch(code);
            isCreated = service.CheckMatchExistence(code);

            Assert.True(isCreated);
        }

        [Fact]
        public void CheckMatchExistenceUnhappyPathTest()
        {
            bool isCreated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isCreated = service.CheckMatchExistence(0);

            Assert.False(isCreated);
        }
    }
}
