using System.ServiceModel;
using System.Windows;
using Parlis.Client.Resources;
using Parlis.Client.Services;

namespace Parlis.Client.Views
{
    public partial class SendRealTimeMessageWindow : Window, IChatManagementCallback
    {
        private readonly ChatManagementClient chatManagementClient;
        private string username;
        private int code;

        public SendRealTimeMessageWindow()
        {
            InitializeComponent();
            chatManagementClient = new ChatManagementClient(new InstanceContext(this));
        }

        public void ConfigureWindow(string username, int code)
        {
            this.code = code;
            this.username = username;
            try
            {
                chatManagementClient.ConnectToChat(username, code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        public void ReceiveMessages(Message[] messages)
        {
            ChatTextBox.Clear();
            foreach (var message in messages)
            {
                ChatTextBox.AppendText(message.PlayerProfileUsername + ": " + message.Content + "\n");
            }
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
            {
                var message = new Message
                {
                    Content = MessageTextBox.Text,
                    PlayerProfileUsername = username,
                };
                MessageTextBox.Clear();
                try
                {
                    chatManagementClient.SendMessage(code, message);
                }
                catch
                (EndpointNotFoundException)
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                }           
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void SendRealTimeMessageWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatManagementClient.DisconnectFromChat(username);
            chatManagementClient.Close();
        }
    }
}