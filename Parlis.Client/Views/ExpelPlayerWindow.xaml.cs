using Parlis.Client.Resources;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ExpelPlayerWindow : Window
    {
        private string username;
        private string[] playerProfiles;

        public ExpelPlayerWindow()
        {
            InitializeComponent();
        }

        public void ConfigureWindow(string username, string[] playerProfiles)
        {
            this.username = username;
            this.playerProfiles = playerProfiles;
            ConfigureData();
        }

        private void ConfigureData()
        {
            foreach (var playerProfile in playerProfiles)
            {
                if (!playerProfile.Equals(username))
                {
                    UsernameComboBox.Items.Add(playerProfile);
                }
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var username = UsernameComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(username))
            {
                Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            Close();
        }
    }
}