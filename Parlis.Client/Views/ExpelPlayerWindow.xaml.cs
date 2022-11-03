using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class ExpelPlayerWindow : Window, IMatchManagementCallback
    {
        private readonly MatchManagementClient matchManagementClient;
        private CreateMatchWindow createMatchWindow;
        private string username;

        public ExpelPlayerWindow()
        {
            InitializeComponent();
            matchManagementClient = new MatchManagementClient(new InstanceContext(this));
        }

        public void ConfigureWindow(CreateMatchWindow createMatchWindow, string username, int code)
        {
            this.createMatchWindow = createMatchWindow;
            this.username = username;
            try
            {
                matchManagementClient.GetPlayerProfiles(username, code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        public void ReceivePlayerProfiles(string[] playerProfiles)
        {
            ConfigureData(playerProfiles);
        }

        private void ConfigureData(string[] playerProfiles)
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