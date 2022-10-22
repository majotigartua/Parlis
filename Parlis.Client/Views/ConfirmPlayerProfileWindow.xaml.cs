using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ConfirmPlayerProfileWindow : Window
    {
        private PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public ConfirmPlayerProfileWindow()
        {
            InitializeComponent();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
            code = Utilities.GenerateRandomCode();
            SendMail();
        }

        private void SendMail()
        {
            string title = Properties.Resources.CONFIRM_PLAYER_PROFILE_WINDOW_TITLE;
            string message = Properties.Resources.CODE_EMAIL_ADDRESS_LABEL;
            try
            {
                if (!playerProfileManagementClient.SendMail(playerProfile, title, message, code))
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                }
            } catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CodeTextBox.Text))
            {
                if (Int32.Parse(CodeTextBox.Text).Equals(code))
                {
                    VerifyPlayerProfile();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    CodeTextBox.Clear();
                }
            } else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void VerifyPlayerProfile()
        {
            playerProfile.IsVerified = true;
            if (playerProfileManagementClient.UpdatePlayerProfile(playerProfile))
            {
                MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                playerProfileManagementClient.Close();

            } else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            Close();
        }
    }
}