using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class JoinMatchWindow : Window
    {
        private PlayerProfile playerProfile;

        public JoinMatchWindow()
        {
            InitializeComponent();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }
    }
}