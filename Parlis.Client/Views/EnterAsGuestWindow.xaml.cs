using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class EnterAsGuestWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private LoginWindow loginWindow;

        public EnterAsGuestWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }
    
        public void ConfigureView(LoginWindow loginWindow)
        {
            this.loginWindow = loginWindow;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrWhiteSpace(username))
            {
                if (!Utilities.ValidateTextLengthOverflowed(username, Constants.MAXIUM_USERNAME_LENGTH))
                {
                    RegisterPlayerProfile(username);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
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
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(username))
                {
                    var playerProfile = new PlayerProfile
                    {
                        Username = username,
                    };
                    if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile))
                    {
                        playerProfileManagementClient.Close();
                        loginWindow.Close();
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
            catch (CommunicationException)
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
            Utilities.PlayButtonClickSound();
            playerProfileManagementClient.Close();
            Close();
        }
    }
}