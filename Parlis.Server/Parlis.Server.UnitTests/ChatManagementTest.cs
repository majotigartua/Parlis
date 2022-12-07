using Parlis.Server.Service.Data;
using System.Collections.Generic;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class ChatManagementTest
    {
        private readonly BusinessLogic.Service service = new BusinessLogic.Service();
        private readonly int code = 123456;
        private readonly PlayerProfile playerProfile = new PlayerProfile
        {
            Username = "testguy"
        };

        [Fact]
        public void GetMessagesSuccessTest()
        {
            string username = playerProfile.Username;
            Message message = new Message
            {
                Content = "Ready to play?",
                PlayerProfileUsername = username,
            };
            service.GetMessages(code);
            service.SendMessage(code, message);
            List<Message> chat = service.GetMessages(code);
            Assert.NotEmpty(chat);
        }
    }
}