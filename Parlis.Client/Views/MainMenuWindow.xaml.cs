using System.Windows;

namespace Parlis.Client.Views
{
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }
        private void CreateMatchButtonClick(object sender, RoutedEventArgs e)
        {
            var createMatchWindow = new CreateMatchWindow();
            Close();
            createMatchWindow.Show();
        }

        private void JoinMatchButtonClick(object sender, RoutedEventArgs e)
        {
            var joinMatchWindow = new JoinMatchWindow();
            Close();
            joinMatchWindow.Show();
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var editPlayerProfileWindow = new EditPlayerProfileWindow();
            Close();
            editPlayerProfileWindow.Show();
        }

        private void GoBackPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        private void SettingsPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var gameConfigurationWindow = new GameConfigurationWindow();
            Close();
            gameConfigurationWindow.Show();
        }

    }
}