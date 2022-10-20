using System.Windows;

namespace Parlis.Client.Views
{
    public partial class EditPlayerProfileWindow : Window
    {
        public EditPlayerProfileWindow()
        {
            InitializeComponent();
        }

        private void ProfilePictureMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmPlayerProfileButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void DeletePlayerProfileClick(object sender, RoutedEventArgs e)
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