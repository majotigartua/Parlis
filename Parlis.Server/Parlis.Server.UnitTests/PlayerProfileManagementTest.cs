using Parlis.Server.Service.Data;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class PlayerProfileManagementTest
    {
        private readonly BusinessLogic.Service service = new BusinessLogic.Service();
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
        public void CheckPlayerExistenceFailedTest()
        {
            bool isRegistered = service.CheckPlayerExistence(null);
            Assert.False(isRegistered);
        }

        [Fact]
        public void CheckPlayerExistenceSuccessTest()
        {
            string emailAddress = player.EmailAddress;
            bool isRegistered = service.CheckPlayerExistence(emailAddress);
            Assert.True(isRegistered);
        }

        [Fact]
        public void CheckPlayerProfileExistenceFailedTest()
        {
            bool isRegistered = service.CheckPlayerProfileExistence(null);
            Assert.False(isRegistered);
        }

        [Fact]
        public void CheckPlayerProfileExistenceSuccessTest()
        {
            string username = playerProfile.Username;
            bool isRegistered = service.CheckPlayerProfileExistence(username);
            Assert.True(isRegistered);
        }

        [Fact]
        public void DeletePlayerFailedTest()
        {
            bool isDeleted = service.DeletePlayer(null);
            Assert.False(isDeleted);
        }

        [Fact]
        public void DeletePlayerSuccessTest()
        {
            string emailAddress = player.EmailAddress;
            bool isDeleted = service.DeletePlayer(emailAddress);
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePlayerProfileFailedTest()
        {
            bool isDeleted = service.DeletePlayerProfile(null);
            Assert.False(isDeleted);
        }

        [Fact]
        public void DeletePlayerProfileSuccessTest()
        {
            string username = playerProfile.Username;
            bool isDeleted = service.DeletePlayerProfile(username);
            Assert.True(isDeleted);
        }

        [Fact]
        public void GetPlayerFailedTest()
        {
            Player player = service.GetPlayer(null);
            Assert.False(this.player.Equals(player));
        }

        [Fact]
        public void GetPlayerSuccessTest()
        {
            string username = playerProfile.Username;
            Player player = service.GetPlayer(username);
            Assert.True(this.player.Equals(player));
        }

        [Fact]
        public void GetPlayerProfileFailedTest()
        {
            PlayerProfile playerProfile = service.GetPlayerProfile(null);
            Assert.False(this.playerProfile.Equals(playerProfile));
        }

        [Fact]
        public void GetPlayerProfileSuccessTest()
        {
            string emailAddress = player.EmailAddress;
            PlayerProfile playerProfile = service.GetPlayerProfile(emailAddress);
            Assert.True(this.playerProfile.Equals(playerProfile));
        }

        [Fact]
        public void LoginFailedTest()
        {
            PlayerProfile playerProfile = service.Login(null, null);
            Assert.False(this.playerProfile.Equals(playerProfile));
        }

        [Fact]
        public void LoginSuccessTest()
        {
            string username = this.playerProfile.Username;
            string password = this.playerProfile.Password;
            PlayerProfile playerProfile = service.Login(username, password);
            Assert.True(this.playerProfile.Equals(playerProfile));
        }

        [Fact]
        public void RegisterPlayerFailedTest()
        {
            bool isRegistered = service.RegisterPlayer(null);
            Assert.False(isRegistered);
        }

        [Fact]
        public void RegisterPlayerSuccessTest()
        {
            bool isRegistered = service.RegisterPlayer(player);
            Assert.True(isRegistered);
        }

        [Fact]
        public void RegisterPlayerProfileFailedTest()
        {
            bool isRegistered = service.RegisterPlayerProfile(null);
            Assert.False(isRegistered);
        }

        [Fact]
        public void RegisterPlayerProfileSuccessTest()
        {
            bool isRegistered = service.RegisterPlayerProfile(playerProfile);
            Assert.True(isRegistered);
        }

        [Fact]
        public void UpdatePlayerFailedTest()
        {
            bool isUpdated = service.UpdatePlayer(null);
            Assert.False(isUpdated);
        }

        [Fact]
        public void UpdatePlayerSuccessTest()
        {
            bool isUpdated = service.UpdatePlayer(player);
            Assert.True(isUpdated);
        }

        [Fact]
        public void UpdatePlayerProfileFailedTest()
        {
            bool isUpdated = service.UpdatePlayerProfile(null);
            Assert.False(isUpdated);
        }

        [Fact]
        public void UpdatePlayerProfileSuccessTest()
        {
            bool isUpdated = service.UpdatePlayerProfile(playerProfile);
            Assert.True(isUpdated);
        }
    }
}