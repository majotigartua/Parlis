using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class RecoverPasswordEmailAddressWindow : Window
    {
        private PlayerProfileManagementClient playerProfileManagementClient;

        public RecoverPasswordEmailAddressWindow()
        {
            InitializeComponent();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        private void RecoverPasswordButtonClick(object sender, RoutedEventArgs e)
        {
            var emailAddress = EmailAddressTextBox.Text;
            if (!string.IsNullOrEmpty(emailAddress))
            {
                if (!Utilities.ValidateEmailAddressFormat(emailAddress))
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
                else if(!playerProfileManagementClient.CheckPlayerExistence(emailAddress))
                {
                    MessageBox.Show(Properties.Resources.EMAIL_ADDRESS_NOT_FOUND_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
                else
                {
                    var recoverPasswordWindow = new RecoverPasswordWindow();
                    Close();
                    var playerProfile = playerProfileManagementClient.GetPlayerProfile(emailAddress);
                    recoverPasswordWindow.ConfigureWindow(playerProfile);
                    recoverPasswordWindow.Show();
                }
            } 
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}