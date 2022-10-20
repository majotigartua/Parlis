using System.Windows;

namespace Parlis.Client.Views
{
    public partial class RecoverPasswordWindow : Window
    {
        public RecoverPasswordWindow()
        {
            InitializeComponent();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}