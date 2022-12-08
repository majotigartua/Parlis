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

        public RegisterPlayerProfileWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            string playerProfilePath = Utilities.SelectProfilePicture();
            if (!string.IsNullOrEmpty(playerProfilePath))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(playerProfilePath);
                bitmapImage.EndInit();
                ProfilePicture.Source = bitmapImage;
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (!ValidateEmptyFields())
            {
                var password = PasswordBox.Password.ToString();
                var emailAddress = EmailAddressTextBox.Text;
                if (Utilities.ValidatePasswordFormat(password) && Utilities.ValidateEmailAddressFormat(emailAddress) && !ValidateTextLengthOverflowed())
                {
                    var username = UsernameTextBox.Text.Replace(" ", "").ToLower();
                    try
                    {
                        CheckPlayerExistence(username, emailAddress);
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

        private bool ValidateTextLengthOverflowed()
        {
            return Utilities.ValidateTextLengthOverflowed(NameTextBox.Text, Constants.MAXIUM_NORMAL_TEXTS_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(PaternalSurnameTextBox.Text, Constants.MAXIUM_NORMAL_TEXTS_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(MaternalSurnameTextBox.Text, Constants.MAXIUM_NORMAL_TEXTS_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(UsernameTextBox.Text, Constants.MAXIUM_USERNAME_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(EmailAddressTextBox.Text, Constants.MAXIUM_EMAIL_ADDRESS_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(PasswordBox.Password.ToString(), Constants.MAXIUM_PASSWORD_LENGTH);
        }

        private void CheckPlayerExistence(string username, string emailAddress)
        {
            if (!playerProfileManagementClient.CheckPlayerProfileExistence(username) && !playerProfileManagementClient.CheckPlayerExistence(emailAddress))
            {
                var password = Utilities.ComputeSHA256Hash(PasswordBox.Password.ToString());
                if (RegisterPlayerProfile(username, password) && RegisterPlayer(emailAddress))
                {
                    Utilities.SaveProfilePicture(username, ProfilePicture);
                    MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                }
                playerProfileManagementClient.Close();
                Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_REGISTERED_WINDOW_TITLE
                    + " "
                    + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
            }
        }

        private bool RegisterPlayerProfile(string username, string password)
        {
            var isRegistered = false;
            playerProfile = new PlayerProfile
            {
                Username = username,
                Password = password,
                IsVerified = false
            };
            if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile))
            {
                isRegistered = true;
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
            }
            return isRegistered;
        }

        private bool RegisterPlayer(string emailAddress)
        {
            var isRegistered = false;
            var player = new Player
            {
                EmailAddress = emailAddress,
                Name = NameTextBox.Text,
                PaternalSurname = PaternalSurnameTextBox.Text,
                MaternalSurname = MaternalSurnameTextBox.Text,
                PlayerProfileUsername = playerProfile.Username,
            };
            if (playerProfileManagementClient.RegisterPlayer(player))
            {
                isRegistered = true;
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
            }
            return isRegistered;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            playerProfileManagementClient.Close();
            Close();
        }
    }
}