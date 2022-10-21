using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class GameConfigurationWindow : Window
    {
        private PlayerProfile playerProfile;

        public GameConfigurationWindow()
        {
            InitializeComponent();
        }

        public void ConfigureView(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMainMenu();
        }

        private void EsMXFlagPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void EnUSFlagPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PtBRFlagPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }
    }
}