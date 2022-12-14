using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class RecoverPasswordWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public RecoverPasswordWindow()
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
            string title = Properties.Resources.RECOVER_PASSWORD_WINDOW_TITLE;
            string message = Properties.Resources.CODE_EMAIL_ADDRESS_LABEL;
            try
            {
                if (!playerProfileManagementClient.SendMail(playerProfile.Username, title, message, code))
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                }
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var password = PasswordBox.Password.ToString();
            if (!string.IsNullOrWhiteSpace(CodeTextBox.Text) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    if (int.Parse(CodeTextBox.Text).Equals(code) && Utilities.ValidatePasswordFormat(password) && Utilities.ValidateTextLengthOverflowed(password, Constants.MAXIUM_PASSWORD_LENGTH))
                    {
                        password = Utilities.ComputeSHA256Hash(password);
                        UpdatePlayerProfile(password);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                        CodeTextBox.Clear();
                        PasswordBox.Clear();
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

        private void UpdatePlayerProfile(string password)
        {
            playerProfile.Password = password;
            if (playerProfileManagementClient.UpdatePlayerProfile(playerProfile))
            {
                MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                playerProfileManagementClient.Close();
                Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
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