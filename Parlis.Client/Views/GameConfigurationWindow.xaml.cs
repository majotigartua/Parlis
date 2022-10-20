using System.Windows;

namespace Parlis.Client.Views
{
    public partial class GameConfigurationWindow : Window
    {
        public GameConfigurationWindow()
        {
            InitializeComponent();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            Close();
            mainMenuWindow.Show();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            Close();
            mainMenuWindow.Show();
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
    }
}