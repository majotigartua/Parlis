using System.ServiceModel;
using System.Windows;
using Parlis.Client.Services;

namespace Parlis.Client.Views
{
    public partial class SendRealTimeMessageWindow : Window, IChatManagementCallback
    {
        private readonly ChatManagementClient chatManagementClient;
        private int code;
        private PlayerProfile playerProfile;

        public SendRealTimeMessageWindow()
        {
            InitializeComponent();
            InstanceContext instanceContext = new InstanceContext(this);
            chatManagementClient = new ChatManagementClient(instanceContext);
        }

        public void ConfigureWindow(int code, PlayerProfile playerProfile)
        {
            this.code = code;
            this.playerProfile = playerProfile;
            try
            {
                chatManagementClient.CreateChat(code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        public void ReceiveMessages(Message[] messages)
        {
            foreach (var message in messages)
            {
                ChatTextBox.Text += message.ToString();
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
            {
                string username = playerProfile.Username;
                var message = new Message
                {
                    Content = MessageTextBox.Text,
                    Username = username,
                };
                chatManagementClient.SendMessage(code, message);
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }
    }
}