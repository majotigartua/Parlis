using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class EditPlayerProfileWindow : Window
    {
        private PlayerProfile playerProfile;

        public EditPlayerProfileWindow()
        {
            InitializeComponent();
            UsernameTextBox.IsEnabled = false;
            EmailAddressTextBox.IsEnabled = false;
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
            var confirmPlayerProfileWindow = new ConfirmPlayerProfileWindow();
            confirmPlayerProfileWindow.ConfigureWindow(playerProfile);
            confirmPlayerProfileWindow.Show();
        }

        private void DeletePlayerProfileClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}