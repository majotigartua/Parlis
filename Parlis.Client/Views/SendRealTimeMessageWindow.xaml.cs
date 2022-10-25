using System.Windows;

namespace Parlis.Client.Views
{
    public partial class SendRealTimeMessageWindow : Window
    {
        public SendRealTimeMessageWindow()
        {
            InitializeComponent();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
