using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {

            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password.ToString();

            if (String.IsNullOrEmpty(username) || (String.IsNullOrEmpty(password)))
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
            else
            {
                Login(username, password);
            }
        }

        private void EnterAsGuestButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RegisterPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            var registerPlayerProfileWindow = new RegisterPlayerProfileWindow();
            registerPlayerProfileWindow.Show();
        }

        private void Login(string username, string password)
        {
            PlayerProfileManagementClient playerProfileManagementClient = new PlayerProfileManagementClient();

            try
            {
                if (playerProfileManagementClient.Login(username, password))
                {
                    playerProfileManagementClient.Close();
                    this.Close();
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
                MessageBox.Show(Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE,
                    Properties.Resources.TRY_AGAIN_LATER_LABEL);
            }
        }
    }
}