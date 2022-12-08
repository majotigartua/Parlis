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
            player.EmailAddress = "testplayer@gmail.com";
            string emailAddress = player.EmailAddress;
            bool isRegistered = service.CheckPlayerExistence(emailAddress);
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
            playerProfile.Username = "guytest";
            string username = playerProfile.Username;
            bool isRegistered = service.CheckPlayerProfileExistence(username);
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
            player.EmailAddress = "testplayer@gmail.com";
            string emailAddress = player.EmailAddress;
            bool isDeleted = service.DeletePlayer(emailAddress);
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
            playerProfile.Username = "guytest";
            string username = playerProfile.Username;
            bool isDeleted = service.DeletePlayerProfile(username);
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
            player.EmailAddress = "testplayer@gmail.com";
            string emailAddress = player.EmailAddress;
            Player playerTest = service.GetPlayer(emailAddress);
            Assert.False(player.Equals(playerTest));
        }

        [Fact]
        public void GetPlayerSuccessTest()
        {
            string username = playerProfile.Username;
            Player playerTest = service.GetPlayer(username);
            Assert.True(player.Equals(playerTest));
        }

        [Fact]
        public void GetPlayerProfileFailedTest()
        {
            playerProfile.Username = "guytest";
            string username = playerProfile.Username;
            PlayerProfile playerProfileTest = service.GetPlayerProfile(username);
            Assert.False(playerProfile.Equals(playerProfileTest));
        }

        [Fact]
        public void GetPlayerProfileSuccessTest()
        {
            string emailAddress = player.EmailAddress;
            PlayerProfile playerProfileTest = service.GetPlayerProfile(emailAddress);
            Assert.True(playerProfile.Equals(playerProfileTest));
        }

        [Fact]
        public void LoginFailedTest()
        {
            playerProfile.Username = "guytest";
            string username = playerProfile.Username;
            string password = playerProfile.Password;
            PlayerProfile playerProfileTest = service.Login(username, password);
            Assert.False(playerProfile.Equals(playerProfileTest));
        }

        [Fact]
        public void LoginSuccessTest()
        {
            string username = playerProfile.Username;
            string password = playerProfile.Password;
            PlayerProfile playerProfileTest = service.Login(username, password);
            Assert.True(playerProfile.Equals(playerProfileTest));
        }

        [Fact]
        public void RegisterPlayerFailedTest()
        {
            player.EmailAddress = null;
            bool isRegistered = service.RegisterPlayer(player);
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
            playerProfile.Username = null;
            bool isRegistered = service.RegisterPlayerProfile(playerProfile);
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
            player.EmailAddress = "testplayer@gmail.com";
            bool isUpdated = service.UpdatePlayer(player);
            Assert.False(isUpdated);
        }

        [Fact]
        public void UpdatePlayerSuccessTest()
        {
            player.EmailAddress = "testplayer@outlook.com";
            bool isUpdated = service.UpdatePlayer(player);
            Assert.True(isUpdated);
        }

        [Fact]
        public void UpdatePlayerProfileFailedTest()
        {
            playerProfile.Username = "guytest";
            bool isUpdated = service.UpdatePlayerProfile(playerProfile);
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