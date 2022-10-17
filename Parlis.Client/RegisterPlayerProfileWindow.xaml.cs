using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Parlis.Client
{
    public partial class RegisterPlayerProfileWindow : Window
    {
        public RegisterPlayerProfileWindow()
        {
            InitializeComponent();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProfilePictureMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password.ToString();
            var emailAddress = EmailAddressTextBox.Text;
            if (!ValidateEmptyFields())
            {
                if (Utilities.ValidatePasswordFormat(password) && Utilities.ValidateEmailAddressFormat(emailAddress))
                {
                    RegisterPlayerProfile(password);
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

        private void RegisterPlayerProfile(string password)
        {
            var username = UsernameTextBox.Text;
            password = Utilities.ComputeSHA256Hash(password);
            var playerProfileManagementClient = new PlayerProfileManagementClient();
            try
            {
                if (!playerProfileManagementClient.CheckPlayerProfileExistence(username))
                {
                    playerProfileManagementClient.Close();
                    UsernameTextBox.IsEnabled = false;
                    var playerProfile = new PlayerProfile
                    {
                        Username = username,
                        Password = password,
                        IsVerified = false
                    };
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
                MessageBox.Show(Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE,
                    Properties.Resources.TRY_AGAIN_LATER_LABEL);
            }
        }

        private void RegisterPlayer(PlayerProfile playerProfile)
        {
            var emailAddress = EmailAddressTextBox.Text;
            var playerProfileManagementClient = new PlayerProfileManagementClient();
            try
            {
                if (!playerProfileManagementClient.CheckPlayerExistence(emailAddress))
                {
                    var player = new Player
                    {
                        Name = NameTextBox.Text,
                        PaternalSurname = PaternalSurnameTextBox.Text,
                        MaternalSurname = MaternalSurnameTextBox.Text,
                        EmailAddress = emailAddress,
                        PlayerProfileUsername = playerProfile.Username
                    };
                    if (playerProfileManagementClient.RegisterPlayerProfile(playerProfile) &&
                        playerProfileManagementClient.RegisterPlayer(player))
                    {
                        playerProfileManagementClient.Close();
                        MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                        this.Close();
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
                MessageBox.Show(Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE,
                    Properties.Resources.TRY_AGAIN_LATER_LABEL);
            }
        }
    }
}