using Microsoft.Win32;
using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class RegisterPlayerProfileWindow : Window
    {
        private PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private string profilePicturePath;

        public RegisterPlayerProfileWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = Properties.Resources.PROFILE_PICTURE_WINDOW_TITLE,
                Filter = "Joint Photographic Experts Group (JPEG)|*.jpg"
            };
            openFileDialog.ShowDialog();
            profilePicturePath = openFileDialog.FileName;
            if (!string.IsNullOrEmpty(profilePicturePath))
            {
                ProfilePicture.Source = new BitmapImage(new Uri(profilePicturePath));
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
            playerProfile = new PlayerProfile
            {
                Username = username,
                Password = password,
                IsVerified = false
            };
            try
            {
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(playerProfile))
                {
                    UsernameTextBox.IsEnabled = false;
                    RegisterPlayer();
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

        private void RegisterPlayer()
        {
            var player = new Player
            {
                Name = NameTextBox.Text,
                PaternalSurname = PaternalSurnameTextBox.Text,
                MaternalSurname = MaternalSurnameTextBox.Text,
                EmailAddress = EmailAddressTextBox.Text,
                PlayerProfileUsername = playerProfile.Username,
            };
            try
            {
                if (!playerProfileManagementClient.CheckPlayerExistence(player))
                {
                    if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile) && playerProfileManagementClient.RegisterPlayer(player))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE
                            + " " + Properties.Resources.CONFIRM_PLAYER_PROFILE_LABEL, "", MessageBoxButton.OKCancel);
                        if (messageBoxResult == MessageBoxResult.OK)
                        {
                            playerProfileManagementClient.Close();
                            GoToConfirmPlayerProfileWindow();
                        }
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
            Close();
        }

        private void GoToConfirmPlayerProfileWindow()
        {
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            confirmPlayerProfileWindow.ConfigureWindow(playerProfile);
            confirmPlayerProfileWindow.Show();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}