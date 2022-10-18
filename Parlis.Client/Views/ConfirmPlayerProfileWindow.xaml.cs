using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ConfirmPlayerProfileWindow : Window
    {
        public ConfirmPlayerProfileWindow()
        {
            InitializeComponent();
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