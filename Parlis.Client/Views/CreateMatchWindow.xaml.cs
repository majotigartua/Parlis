using Parlis.Client.Services;
using System.Windows;


namespace Parlis.Client.Views
{
    public partial class CreateMatchWindow : Window
    {
        private PlayerProfile playerProfile;

        public CreateMatchWindow()
        {
            InitializeComponent();
        }

        public void ConfigureView(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
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