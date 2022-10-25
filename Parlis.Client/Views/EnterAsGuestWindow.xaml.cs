using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class EnterAsGuestWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;

        public EnterAsGuestWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                RegisterPlayerProfile(username);
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void RegisterPlayerProfile(string username)
        {
            username = username.Replace(" ", "").ToLower();
            try
            {
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(playerProfile.Username))
                {
                    var playerProfile = new PlayerProfile {
                        Username = username,
                    };
                    if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile))
                    {
                        playerProfileManagementClient.Close();
                        GoToMainMenu(playerProfile);
                    }
                    else
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

        private void GoToMainMenu(PlayerProfile playerProfile)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            playerProfileManagementClient.Close();
            Close();
        }
    }
}
