﻿using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class LoginWindow : Window
    {
        private readonly PlayerProfileManagementClient playerProfileManagementClient;

        public LoginWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            UsernameTextBox.Focus();
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }
        private void EnterAsGuestButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var enterAsGuestWindow = new EnterAsGuestWindow();
            enterAsGuestWindow.ConfigureView(this);
            enterAsGuestWindow.ShowDialog();
        }

        private void ForgottenPasswordLabelMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var recoverPasswordEmailAddressWindow = new RecoverPasswordEmailAddressWindow();
            recoverPasswordEmailAddressWindow.ShowDialog();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password.ToString();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
            else
            {
                password = Utilities.ComputeSHA256Hash(password);
                Login(username, password);
            }
        }

        private void Login(string username, string password)
        {
            try
            {
                var playerProfile = playerProfileManagementClient.Login(username, password);
                if (playerProfile != null && !ValidateTextLengthOverflowed())
                {
                    playerProfileManagementClient.Close();
                    GoToMainMenu(playerProfile);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                              Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    PasswordBox.Clear();
                }
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                Close();
            }
        }

        private bool ValidateTextLengthOverflowed()
        {
            return Utilities.ValidateTextLengthOverflowed(UsernameTextBox.Text, Constants.MAXIUM_USERNAME_LENGTH) ||
                Utilities.ValidateTextLengthOverflowed(PasswordBox.Password.ToString(), Constants.MAXIUM_PASSWORD_LENGTH);
        }

        private void GoToMainMenu(PlayerProfile playerProfile)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        private void RegisterPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var registerPlayerProfileWindow = new RegisterPlayerProfileWindow();
            registerPlayerProfileWindow.ShowDialog();
        }
    }
}