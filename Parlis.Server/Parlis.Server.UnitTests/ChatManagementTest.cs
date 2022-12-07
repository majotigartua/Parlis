using Parlis.Server.Service.Data;
using System.Collections.Generic;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class ChatManagementTest
    {

        [Fact]
        public void GetMessagesTest()
        {
            int code = 123456;
            PlayerProfile playerProfile = new PlayerProfile
            {
                Username = "testguy"
            };
            Message message = new Message
            {
                PlayerProfileUsername = playerProfile.Username,
                Content = "Ready to play?"
            };
            BusinessLogic.Service service = new BusinessLogic.Service();
            service.GetMessages(code);
            service.SendMessage(code, message);
            List<Message> chat = service.GetMessages(code);
            Assert.NotEmpty(chat);
        }
    }
}