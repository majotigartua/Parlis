using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class CreateMatchWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private readonly MatchManagementClient matchManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public CreateMatchWindow()
        {
            InitializeComponent();
            playerProfileManagementClient = new PlayerProfileManagementClient();
            matchManagementClient = new MatchManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile, bool isJoinMatch, int code)
        {
            this.playerProfile = playerProfile;
            if (isJoinMatch)
            {
                this.code = code;
            }
            else
            {
                this.code = Utilities.GenerateRandomCode();
                CreateMatch();
            }
            ConfigureData();
        }

        private void CreateMatch()
        {
            try
            {
                matchManagementClient.CreateMatch(code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        public void ConfigureData()
        {
            CodeTextBox.Text = code.ToString();
        }

        private void ExpelPlayerMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var expelPlayerWindow = new ExpelPlayerWindow();
            expelPlayerWindow.ShowDialog();
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            sendRealTimeMessageWindow.ShowDialog();
        }

        private void SendInvitationButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    if (playerProfileManagementClient.CheckPlayerProfileExistence(username))
                    {
                        SendMail(username);
                        playerProfileManagementClient.Close();
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    }
                }
                catch (EndpointNotFoundException)
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

        private void SendMail(string username)
        {
            string title = Properties.Resources.JOIN_MATCH_WINDOW_TITLE;
            string message = Properties.Resources.CODE_EMAIL_ADDRESS_LABEL;
            if (!playerProfileManagementClient.SendMail(username, title, message, code))
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void StartMatchButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            matchManagementClient.Close();
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }
    }
}