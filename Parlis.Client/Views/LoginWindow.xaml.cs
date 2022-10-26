using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class LoginWindow : Window, IMatchManagementCallback
    {
        private readonly MatchManagementClient matchManagementClient;
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;

        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            var instanceContext = new InstanceContext(this);
            matchManagementClient = new MatchManagementClient(instanceContext);
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        private void ForgottenPasswordLabelMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var recoverPasswordEmailAddressWindow = new RecoverPasswordEmailAddressWindow();
            recoverPasswordEmailAddressWindow.ShowDialog();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password.ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
            else
            {
                if (!matchManagementClient.CheckPlayerProfile(username))
                {
                    password = Utilities.ComputeSHA256Hash(password);
                    Login(username, password);
                } 
                else
                {
                    MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_CONNECTED_WINDOW_TITLE
                        + " "
                        + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
                }
            }
        }

        private void Login(string username, string password)
        {
            try
            {
                playerProfile = playerProfileManagementClient.Login(username, password);
                if (playerProfile != null)
                {
                    playerProfileManagementClient.Close();
                    GoToMainMenu();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                              Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    PasswordBox.Clear();
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void EnterAsGuestButtonClick(object sender, RoutedEventArgs e)
        {
            var enterAsGuestWindow =  new EnterAsGuestWindow();
            enterAsGuestWindow.ShowDialog();
        }

        private void RegisterPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        { 
            var registerPlayerProfileWindow = new RegisterPlayerProfileWindow();
            registerPlayerProfileWindow.ShowDialog();
        }

        public void GetPlayerProfiles(string[] playerProfiles)
        {
            throw new System.NotImplementedException();
        }
    }
}