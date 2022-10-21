using Parlis.Client.Services;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ConfirmPlayerProfileWindow : Window
    {
        private PlayerProfile playerProfile;

        public ConfirmPlayerProfileWindow()
        {
            InitializeComponent();
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }
    }
}