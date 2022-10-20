using Microsoft.Win32;
using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Parlis.Client.Views
{
    public partial class RegisterPlayerProfileWindow : Window
    {
        public RegisterPlayerProfileWindow()
        {
            InitializeComponent();
        }

        private void ProfilePictureMouseDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = Properties.Resources.PROFILE_PICTURE_WINDOW_TITLE,
                Filter = "Joint Photographic Experts Group (JPEG)|*.jpeg;*.jpg"
            };
            openFileDialog.ShowDialog();
            if (!openFileDialog.FileName.Equals(null))
            {
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password.ToString();
            var emailAddress = EmailAddressTextBox.Text;
            if (!ValidateEmptyFields())
            {
                if (Utilities.ValidatePasswordFormat(password) && Utilities.ValidateEmailAddressFormat(emailAddress))
                {
                    RegisterPlayerProfile();
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

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidateEmptyFields()
        {
            return string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(PaternalSurnameTextBox.Text) ||
                string.IsNullOrEmpty(MaternalSurnameTextBox.Text) ||
                string.IsNullOrEmpty(UsernameTextBox.Text) ||
                string.IsNullOrEmpty(EmailAddressTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password.ToString());
        }

        private void RegisterPlayerProfile()
        {
            string username = UsernameTextBox.Text.Replace(" ", "").ToLower();
            string password = Utilities.ComputeSHA256Hash(PasswordBox.Password.ToString());
            var playerProfile = new PlayerProfile
            {
                Username = username,
                Password = password,
                IsVerified = false
            };
            try
            {
                var playerProfileManagementClient = new PlayerProfileManagementClient();
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(playerProfile))
                {
                    playerProfileManagementClient.Close();
                    UsernameTextBox.IsEnabled = false;
                    RegisterPlayer(playerProfile);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_REGISTERED_WINDOW_TITLE
                    + " "
                    + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void RegisterPlayer(PlayerProfile playerProfile)
        {
            var player = new Player
            {
                Name = NameTextBox.Text,
                PaternalSurname = PaternalSurnameTextBox.Text,
                MaternalSurname = MaternalSurnameTextBox.Text,
                EmailAddress = EmailAddressTextBox.Text,
                PlayerProfile = playerProfile
            };
            try
            {
                var playerProfileManagementClient = new PlayerProfileManagementClient();
                if (!playerProfileManagementClient.CheckPlayerExistence(player))
                {
                    if (playerProfileManagementClient.RegisterPlayer(player))
                    {
                        playerProfileManagementClient.Close();
                        MessageBoxResult messageBoxResult = MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE
                            + " "
                            + Properties.Resources.CONFIRM_PLAYER_PROFILE_LABEL, "",
                            MessageBoxButton.OKCancel);
                        if (messageBoxResult == MessageBoxResult.OK)
                        {
                            GoToConfirmPlayerProfileWindow();
                        }
                        Close();
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
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void GoToConfirmPlayerProfileWindow()
        {
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            confirmPlayerProfileWindow.Show();
        }
    }
}