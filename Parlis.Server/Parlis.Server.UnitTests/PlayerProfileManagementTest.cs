using Parlis.Server.Service.Data;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class PlayerProfileManagementTest
    {

        private readonly PlayerProfile playerProfile = new PlayerProfile
        {
            Username = "testguy",
            Password = "TrialPassword0",
            IsVerified = true
        };

        private readonly Player player = new Player
        {
            EmailAddress = "testplayer@outlook.com",
            Name = "Regular",
            PaternalSurname = "Test",
            MaternalSurname = "Guy",
            PlayerProfileUsername = "testguy"
        };

        [Fact]
        public void RegisterPlayerProfileTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isPlayerProfileRegistered = service.RegisterPlayerProfile(playerProfile);
            Assert.True(isPlayerProfileRegistered);
        }

        [Fact]
        public void RegisterPlayerTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isRegistered = service.RegisterPlayer(player);
            Assert.True(isRegistered);
        }

        [Fact]
        public void CheckPlayerProfileExistenceTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isRegistered = service.CheckPlayerProfileExistence(playerProfile.Username);
            Assert.True(isRegistered);
        }

        [Fact]
        public void CheckPlayerExistenceTest()
        {
            BusinessLogic.Service service = new Server.BusinessLogic.Service();
            bool isRegistered = service.CheckPlayerExistence(player.EmailAddress);
            Assert.True(isRegistered);
        }

        [Fact]
        public void GetPlayerProfileTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            PlayerProfile playerProfile = service.GetPlayerProfile(player.EmailAddress);
            Assert.True(playerProfile.Equals(this.playerProfile));
        }

        [Fact]
        public void GetPlayerTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            Player player = service.GetPlayer(playerProfile.Username);
            Assert.True(player.Equals(this.player));
        }

        [Fact]
        public void LoginTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            PlayerProfile playerProfile = service.Login(this.playerProfile.Username, this.playerProfile.Password);
            Assert.True(playerProfile.Equals(this.playerProfile));
        }

        [Fact]
        public void UpdatePlayerProfileTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isUpdated = service.UpdatePlayerProfile(playerProfile);
            Assert.True(isUpdated);
        }

        [Fact]
        public void UpdatePlayerTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isUpdated = service.UpdatePlayer(player);
            Assert.True(isUpdated);
        }

        [Fact]
        public void DeletePlayerTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isDeleted = service.DeletePlayer(player.EmailAddress);
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePlayerProfileTest()
        {
            BusinessLogic.Service service = new BusinessLogic.Service();
            bool isDeleted = service.DeletePlayerProfile(playerProfile.Username);
            Assert.True(isDeleted);
        }
    }
}