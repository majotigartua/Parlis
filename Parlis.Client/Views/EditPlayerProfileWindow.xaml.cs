using Microsoft.Win32;
using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class EditPlayerProfileWindow : Window
    {
        private PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private Player player;

        public EditPlayerProfileWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
            UsernameTextBox.IsEnabled = false;
            EmailAddressTextBox.IsEnabled = false;
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
            if ((bool) playerProfile.IsVerified)
            {
                ConfirmPlayerProfileButton.IsEnabled = false;
            }
            ConfigureData();
        }

        public void ConfigureData()
        {
            ProfilePicture.Source = new BitmapImage(new Uri("/Resources/Images/ProfilePictures/" + playerProfile.Username + ".jpg", UriKind.Relative));
            try
            {
                player = playerProfileManagementClient.GetPlayer(playerProfile);
                EmailAddressTextBox.Text = player.EmailAddress;
                NameTextBox.Text = player.Name;
                PaternalSurnameTextBox.Text = player.PaternalSurname;
                MaternalSurnameTextBox.Text = player.MaternalSurname;
                UsernameTextBox.Text = playerProfile.Username;
                PasswordBox.Password = playerProfile.Password.Substring(0, 8);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = Properties.Resources.PROFILE_PICTURE_WINDOW_TITLE,
                Filter = "Joint Photographic Experts Group (JPEG)|*.jpeg;*.jpg"
            };
            openFileDialog.ShowDialog();
            if (!openFileDialog.FileName.Equals(null))
            {
                ProfilePicture.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateEmptyFields())
            {
                try
                {
                    string password = PasswordBox.Password.ToString();
                    if (password.Equals(playerProfile.Password.Substring(0, 8)))
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
                    playerProfileManagementClient.Close();
                    GoToMainMenu();
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
                string.IsNullOrEmpty(MaternalSurnameTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password.ToString());
        }

        private void UpdatePlayer()
        {
            player.Name = NameTextBox.Text;
            player.PaternalSurname = PaternalSurnameTextBox.Text;
            player.MaternalSurname = MaternalSurnameTextBox.Text;
            if (playerProfileManagementClient.UpdatePlayer(player))
            {
                MessageBox.Show(Properties.Resources.REGISTERED_INFORMATION_WINDOW_TITLE);
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                   Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
            }
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

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void DeletePlayerProfileClick(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show(Properties.Resources.DELETE_PLAYER_PROFILE_LABEL, "", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    if (playerProfileManagementClient.DeletePlayer(player) && playerProfileManagementClient.DeletePlayerProfile(playerProfile))
                    {
                        playerProfileManagementClient.Close();
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
        }

        private void GoToLogin()
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        private void ConfirmPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            ConfirmPlayerProfileButton.IsEnabled = false;
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            confirmPlayerProfileWindow.ConfigureWindow(playerProfile);
            confirmPlayerProfileWindow.ShowDialog();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }
    }
}