using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class MainMenuWindow : Window
    {
        private static Player _player;

        public MainMenuWindow()
        {
            InitializeComponent();
        }

        public void ConfigureView(Player player)
        {
            _player = player;
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
            editPlayerProfileWindow.ShowDialog();
        }

        private void ExitPictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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