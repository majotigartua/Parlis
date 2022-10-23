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

        private void EsMXFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-MX");
        }

        private void EnUSFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("");
        }

        private void PtBRFlagMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
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