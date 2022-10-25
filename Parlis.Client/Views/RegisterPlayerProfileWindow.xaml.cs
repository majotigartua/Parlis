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
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
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
            profilePicturePath = Utilities.SelectProfilePicture();
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
                    try
                    {
                        RegisterPlayerProfile(password, emailAddress);
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

        private void RegisterPlayerProfile(string password, string emailAddress)
        {
            string username = UsernameTextBox.Text.Replace(" ", "").ToLower();
            if (!playerProfileManagementClient.CheckPlayerProfileExistence(username))
            {
                password = Utilities.ComputeSHA256Hash(password);
                playerProfile = new PlayerProfile
                {
                    Username = username,
                    Password = password,
                    IsVerified = false
                };
                UsernameTextBox.IsEnabled = false;
                RegisterPlayer(emailAddress);
            } 
            else
            {
                MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_REGISTERED_WINDOW_TITLE
                    + " "
                    + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
            }
        }

        private void RegisterPlayer(string emailAddress)
        {
            if (!playerProfileManagementClient.CheckPlayerExistence(emailAddress))
            {
                var player = new Player
                {
                    EmailAddress = emailAddress,
                    Name = NameTextBox.Text,
                    PaternalSurname = PaternalSurnameTextBox.Text,
                    MaternalSurname = MaternalSurnameTextBox.Text,
                    PlayerProfileUsername = playerProfile.Username,
                };
                if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile) && playerProfileManagementClient.RegisterPlayer(player))
                {
                    var messageBoxResult = MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE + " " + 
                        Properties.Resources.CONFIRM_PLAYER_PROFILE_LABEL, "", MessageBoxButton.OKCancel);
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

        private void GoToConfirmPlayerProfileWindow()
        {
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            confirmPlayerProfileWindow.ConfigureWindow(playerProfile);
            confirmPlayerProfileWindow.Show();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            playerProfileManagementClient.Close();
            Close();
        }
    }
}