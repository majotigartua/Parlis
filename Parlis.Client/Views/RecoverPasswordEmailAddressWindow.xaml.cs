using Parlis.Client.Resources;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class RecoverPasswordEmailAddressWindow : Window
    {
        public RecoverPasswordEmailAddressWindow()
        {
            InitializeComponent();
        }

        private void RecoverPasswordButtonClick(object sender, RoutedEventArgs e)
        {
            var emailAddress = EmailAddressTextBox.Text;
            if(!string.IsNullOrEmpty(emailAddress))
            {
                if(Utilities.ValidateEmailAddressFormat(emailAddress))
                {
                    var recoverPasswordWindow = new RecoverPasswordWindow();
                    this.Visibility = Visibility.Hidden;
                    recoverPasswordWindow.Show();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
            } 
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}