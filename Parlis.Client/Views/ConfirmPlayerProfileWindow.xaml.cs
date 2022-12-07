using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ConfirmPlayerProfileWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public ConfirmPlayerProfileWindow()
        {
            InitializeComponent();
            CodeTextBox.Focus();
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
                if (!playerProfileManagementClient.SendMail(playerProfile.Username, title, message, code))
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
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
            Utilities.PlayButtonClickSound();
            playerProfileManagementClient.Close();
            Close();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (!string.IsNullOrEmpty(CodeTextBox.Text))
            {
                try
                {
                    if (int.Parse(CodeTextBox.Text).Equals(code))
                    {
                        UpdatePlayerProfile();
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                        CodeTextBox.Clear();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    CodeTextBox.Clear();
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void UpdatePlayerProfile()
        {
            playerProfile.IsVerified = true;
            try
            {
                if (playerProfileManagementClient.UpdatePlayerProfile(playerProfile))
                {
                    playerProfileManagementClient.Close();
                    MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            Close();
        }
    }
}