using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class LoginWindow : Window 
    {
        private InstanceContext context;
        private PlayerProfileManagementClient playerProfileManagementClient;
        private MatchManagementClient matchManagementClient;

        public LoginWindow()
        {
            InitializeComponent();
            context = new InstanceContext(this);
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
                password = Utilities.ComputeSHA256Hash(password);
                Login(username, password);
            }
        }

        private void EnterAsGuestButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }

        private void RegisterPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            var registerPlayerProfileWindow = new RegisterPlayerProfileWindow();
            registerPlayerProfileWindow.ShowDialog();
        }

        private void Login(string username, string password)
        {
            var playerProfile = new PlayerProfile
            {
                Username = username,
                Password = password,
            };
            try
            {
                if (playerProfileManagementClient.Login(playerProfile))
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
            Close();
            mainMenuWindow.Show();
        }
    }
}