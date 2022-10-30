using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
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

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var emailAddress = EmailAddressTextBox.Text;
            if (!string.IsNullOrEmpty(emailAddress))
            {
                try
                {
                    if (playerProfileManagementClient.CheckPlayerExistence(emailAddress))
                    {
                        var playerProfile = playerProfileManagementClient.GetPlayerProfile(emailAddress);
                        playerProfileManagementClient.Close();
                        GoToRecoverPassword(playerProfile);
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

        private void GoToRecoverPassword(PlayerProfile playerProfile)
        {
            var recoverPasswordWindow = new RecoverPasswordWindow();
            try
            {
                recoverPasswordWindow.ConfigureWindow(playerProfile);
                Close();
                recoverPasswordWindow.Show();
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            playerProfileManagementClient.Close();
            Close();
        }
    }
}
