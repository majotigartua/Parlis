using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ExpelPlayerWindow : Window
    {
        public ExpelPlayerWindow()
        {
            InitializeComponent();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}