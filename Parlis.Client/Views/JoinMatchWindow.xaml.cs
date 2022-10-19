using System.Windows;

namespace Parlis.Client.Views
{
    public partial class JoinMatchWindow : Window
    {
        public JoinMatchWindow()
        {
            InitializeComponent();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            Close();
            mainMenuWindow.Show();
        }
    }
}