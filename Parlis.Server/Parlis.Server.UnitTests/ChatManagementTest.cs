using Parlis.Server.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Parlis.Server.UnitTests
{
    public class ChatManagementTest
    {
        int code = 123890;
        PlayerProfile playerProfile = new PlayerProfile
        {
            Username = "testguy"
        };

        [Fact]
        public void GetMessagesHappyPathTest()
        {
            List<Message> chat;
            Message message = new Message();
            message.PlayerProfileUsername = playerProfile.Username;
            message.Content = "Ready to play?";

            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            service.GetMessages(code);
            service.SendMessage(code, message);
            chat = service.GetMessages(code);

            Assert.NotEmpty(chat);
        }

        [Fact]
        public void GetMessagesUnhappyPathTest()
        {
            int incorrectCode = 345021;
            List<Message> chat;
            Message message = new Message();
            message.PlayerProfileUsername = playerProfile.Username;
            message.Content = "Just start the match dude!";

            Server.BusinessLogic.Service service = new Server.BusinessLogic.Service();

            service.GetMessages(code);
            service.SendMessage(code, message);
            chat = service.GetMessages(incorrectCode);

            Assert.Empty(chat);

        }
    }
}
