using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class EditPlayerProfileWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private Player player;

        public EditPlayerProfileWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
            Utilities.PlayMusic();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
            ConfirmPlayerProfileButton.IsEnabled = !playerProfile.IsVerified;
            ConfigureData();
        }

        public void ConfigureData()
        {
            string username = playerProfile.Username;
            var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
            try
            {
                player = playerProfileManagementClient.GetPlayer(username);
                EmailAddressTextBox.Text = player.EmailAddress;
                NameTextBox.Text = player.Name;
                PaternalSurnameTextBox.Text = player.PaternalSurname;
                MaternalSurnameTextBox.Text = player.MaternalSurname;
                UsernameTextBox.Text = username;
                ProfilePicture.Source = new BitmapImage(new Uri(profilePicturePath));
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            catch (IOException)
            {
                ProfilePicture.Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (!ValidateEmptyFields())
            {
                var password = PasswordBox.Password.ToString();
                try
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        UpdatePlayer();
                    }
                    else
                    {
                        if (Utilities.ValidatePasswordFormat(password))
                        {
                            password = Utilities.ComputeSHA256Hash(password);
                            UpdatePlayerProfile(password);
                            UpdatePlayer();
                        }
                        else
                        {
                            MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                                Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                        }
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

        private bool ValidateEmptyFields()
        {
            return string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(PaternalSurnameTextBox.Text) ||
                string.IsNullOrEmpty(MaternalSurnameTextBox.Text);
        }

        private void UpdatePlayerProfile(string password)
        {
            playerProfile.Password = password;
            if (!playerProfileManagementClient.UpdatePlayerProfile(playerProfile))
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
            }
        }

        private void UpdatePlayer()
        {
            player.Name = NameTextBox.Text;
            player.PaternalSurname = PaternalSurnameTextBox.Text;
            player.MaternalSurname = MaternalSurnameTextBox.Text;
            if (playerProfileManagementClient.UpdatePlayer(player))
            {
                MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
                GoToMainMenu();
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                   Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
            }
        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void DeletePlayerProfileClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            try
            {
                if (playerProfileManagementClient.DeletePlayer(player.EmailAddress) && playerProfileManagementClient.DeletePlayerProfile(playerProfile.Username))
                {
                    GoToLogin();
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
        }

        private void GoToLogin()
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        private void ConfirmPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            try
            {
                confirmPlayerProfileWindow.ConfigureWindow(playerProfile);
                confirmPlayerProfileWindow.ShowDialog();
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            ConfirmPlayerProfileButton.IsEnabled = false;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            GoToMainMenu();
        }
    }
}