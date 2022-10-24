using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class MainMenuWindow : Window
    {
        private PlayerProfile playerProfile;

        public MainMenuWindow()
        {
            InitializeComponent();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void CreateMatchButtonClick(object sender, RoutedEventArgs e)
        {
            var createMatchWindow = new CreateMatchWindow();
            createMatchWindow.ConfigureView(playerProfile);
            Close();
            createMatchWindow.Show();
        }

        private void JoinMatchButtonClick(object sender, RoutedEventArgs e)
        {
            var joinMatchWindow = new JoinMatchWindow();
            joinMatchWindow.ConfigureWindow(playerProfile);
            Close();
            joinMatchWindow.Show();
        }

        private void PlayerProfileMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var editPlayerProfileWindow = new EditPlayerProfileWindow();
            editPlayerProfileWindow.ConfigureWindow(playerProfile);
            Close();
            editPlayerProfileWindow.Show();
        }

        private void SettingsMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var gameConfigurationWindow = new GameConfigurationWindow();
            gameConfigurationWindow.ConfigureView(playerProfile);
            Close();
            gameConfigurationWindow.Show();
        }

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }
    }
}