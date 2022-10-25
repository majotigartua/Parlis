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

        private void ExpelPlayerMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var expelPlayerWindow = new ExpelPlayerWindow();
            expelPlayerWindow.ShowDialog();
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            sendRealTimeMessageWindow.ShowDialog();
        }
        private void SendInvitationButtonClick(object sender, RoutedEventArgs e)
        { 
        }

        private void StartMatchButtonClick(object sender, RoutedEventArgs e)
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