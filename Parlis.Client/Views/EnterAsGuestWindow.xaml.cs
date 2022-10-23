using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class EnterAsGuestWindow : Window
    {
        private LoginWindow loginWindow;

        public EnterAsGuestWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        public void ConfigureWindow(LoginWindow loginWindow)
        {
            this.loginWindow = loginWindow;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                RegisterPlayerProfile();
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void RegisterPlayerProfile()
        {
            string username = UsernameTextBox.Text.Replace(" ", "").ToLower();
            var playerProfile = new PlayerProfile
            {
                Username = username,
            };
            try
            {
                var playerProfileManagementClient = new PlayerProfileManagementClient();
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(playerProfile))
                {
                    if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile)) {
                        playerProfileManagementClient.Close();
                        loginWindow.ConfigureWindow(playerProfile);
                        Close();
                    } else
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_REGISTERED_WINDOW_TITLE
                    + " "
                    + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}