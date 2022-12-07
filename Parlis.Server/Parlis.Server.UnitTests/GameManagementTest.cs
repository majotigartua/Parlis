using Parlis.Server.Service.Data;
using System;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class GameManagementTest
    {
        private readonly BusinessLogic.Service service = new BusinessLogic.Service();
        private readonly Match match = new Match
        {
            Date = DateTime.Now,
            PlayerProfileUsername = "testguy"
        };

        [Fact]
        public void RegisterMatchFailedTest()
        {
            bool isRegistered = service.RegisterMatch(null);
            Assert.False(isRegistered);
        }

        [Fact]
        public void RegisterMatchSuccessTest()
        {

            bool isRegistered = service.RegisterMatch(match);
            Assert.True(isRegistered);
        }
    }
}