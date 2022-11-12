using Parlis.Server.BusinessLogic;
using Parlis.Server.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class PlayerProfileManagementTest
    {
        private PlayerProfile playerProfile = new PlayerProfile
        {
            Username = "testguy",
            Password = "TrialPassword0",
            IsVerified = true
        };

        private Player player = new Player
        {
            EmailAddress = "testplayer@outlook.com",
            Name = "Regular",
            PaternalSurname = "Test",
            MaternalSurname = "Guy",
            PlayerProfileUsername = "testguy"
        };

        [Fact]
        public void RegisterPlayerProfileHappyPathTest()
        {
            bool isRegistered = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isRegistered = service.RegisterPlayerProfile(playerProfile);

            Assert.True(isRegistered);
        }

        [Fact]
        public void RegisterPlayerProfileUnhappyPathTest()
        {
            bool isRegistered = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isRegistered = service.RegisterPlayerProfile(null);

            Assert.False(isRegistered);
        }

        [Fact]
        public void RegisterPlayerHappyPathTest()
        {
            bool isRegistered = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isRegistered = service.RegisterPlayer(player);

            Assert.True(isRegistered);
        }

        [Fact]
        public void RegisterPlayerUnhappyPathTest()
        {
            bool isRegistered = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isRegistered = service.RegisterPlayer(null);

            Assert.False(isRegistered);
        }

        [Fact]
        public void CheckPlayerProfileExistenceHappyPathTest()
        {
            bool isSelected = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSelected = service.CheckPlayerProfileExistence(playerProfile.Username);

            Assert.True(isSelected);
        }

        [Fact]
        public void CheckPlayerProfileExistenceUnhappyPathTest()
        {
            bool isSelected = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSelected = service.CheckPlayerProfileExistence(null);

            Assert.False(isSelected);
        }

        [Fact]
        public void CheckPlayerExistenceHappyPathTest()
        {
            bool isSelected = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSelected = service.CheckPlayerExistence(player.EmailAddress);

            Assert.True(isSelected);
        }

        [Fact]
        public void CheckPlayerExistenceUnhappyPathTest()
        {
            bool isSelected = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSelected = service.CheckPlayerExistence(null);

            Assert.False(isSelected);
        }

        [Fact]
        public void GetPlayerProfileHappyPathTest()
        {
            PlayerProfile testPlayerProfile = new PlayerProfile();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayerProfile = service.GetPlayerProfile(player.EmailAddress);

            Assert.True(testPlayerProfile.Equals(playerProfile));
        }

        [Fact]
        public void GetPlayerProfileUnhappyPathTest()
        {
            PlayerProfile testPlayerProfile = new PlayerProfile();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayerProfile = service.GetPlayerProfile(null);

            Assert.Null(testPlayerProfile);
        }

        [Fact]
        public void GetPlayerHappyPathTest()
        {
            Player testPlayer = new Player();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayer = service.GetPlayer(playerProfile.Username);

            Assert.True(testPlayer.Equals(player));
        }

        [Fact]
        public void GetPlayerUnhappyPathTest()
        {
            Player testPlayer = new Player();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayer = service.GetPlayer(null);

            Assert.Null(testPlayer);
        }

        [Fact]
        public void LoginHappyPathTest()
        {
            PlayerProfile testPlayerProfile = new PlayerProfile();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayerProfile = service.Login(playerProfile.Username, playerProfile.Password);

            Assert.True(testPlayerProfile.Equals(playerProfile));
        }

        [Fact]
        public void LoginUnhappyPathTest()
        {
            PlayerProfile testPlayerProfile = new PlayerProfile();
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            testPlayerProfile = service.Login(null, null);

            Assert.Null(testPlayerProfile);
        }

        [Fact]
        public void UpdatePlayerProfileHappyPathTest()
        {
            bool isUpdated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isUpdated = service.UpdatePlayerProfile(playerProfile);

            Assert.True(isUpdated);
        }

        [Fact]
        public void UpdatePlayerProfileUnhappyPathTest()
        {
            bool isUpdated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isUpdated = service.UpdatePlayerProfile(null);

            Assert.False(isUpdated);
        }

        [Fact]
        public void UpdatePlayerHappyPathTest()
        {
            bool isUpdated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isUpdated = service.UpdatePlayer(player);

            Assert.True(isUpdated);
        }

        [Fact]
        public void UpdatePlayerUnhappyPathTest()
        {
            bool isUpdated = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isUpdated = service.UpdatePlayer(null);

            Assert.False(isUpdated);
        }

        [Fact]
        public void SendEmailHappyPathTest()
        {
            bool isSent = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSent = service.SendMail(playerProfile.Username, "Test", "Please, do not reply.", 112233);

            Assert.True(isSent); // ArgumentNullException threw on line 223.
        }

        [Fact]
        public void SendEmailUnHappyPathTest()
        {
            bool isSent = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isSent = service.SendMail(null, null, null, 112233);

            Assert.False(isSent);
        }

        [Fact]
        public void DeletePlayerHappyPathTest()
        {
            bool isDeleted = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isDeleted = service.DeletePlayer(player.EmailAddress);

            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePlayerUnhappyPathTest()
        {
            bool isDeleted = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isDeleted = service.DeletePlayer(null);

            Assert.False(isDeleted);
        }

        [Fact]
        public void DeletePlayerProfileHappyPathTest()
        {
            bool isDeleted = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isDeleted = service.DeletePlayerProfile(playerProfile.Username);

            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePlayerProfileUnhappyPathTest()
        {
            bool isDeleted = false;
            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            isDeleted = service.DeletePlayerProfile(null);

            Assert.False(isDeleted);
        }
    }
}
