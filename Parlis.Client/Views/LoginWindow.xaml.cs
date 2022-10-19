using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ForgottenPasswordLabelMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
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
            Close();
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
                var playerProfileManagementClient = new PlayerProfileManagementClient();
                if (playerProfileManagementClient.Login(playerProfile))
                {
                    playerProfileManagementClient.Close();
                    Close();
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
    }
}