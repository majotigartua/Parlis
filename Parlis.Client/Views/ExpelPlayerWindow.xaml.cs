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
        private PlayerProfile playerProfile;

        public ExpelPlayerWindow()
        {
            InitializeComponent();
            matchManagementClient = new MatchManagementClient(new InstanceContext(this));
        }

        public void ConfigureWindow(CreateMatchWindow createMatchWindow, PlayerProfile playerProfile, int code)
        {
            this.createMatchWindow = createMatchWindow;
            this.playerProfile = playerProfile;
            string username = playerProfile.Username;
            try
            {
                matchManagementClient.GetPlayerProfiles(username, code);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }
        public void ExpelPlayerProfileFromMatch(string username)
        {
        }

        public void ReceivePlayerProfiles(string[] playerProfiles)
        {
            ConfigureData(playerProfiles);
        }

        private void ConfigureData(string[] playerProfiles)
        {
            string username = playerProfile.Username;
            foreach (var playerProfile in playerProfiles)
            {
                if (!playerProfile.Equals(username))
                {
                    UsernameComboBox.Items.Add(playerProfile);
                }
            }
        }

        public void StartMatch()
        {
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var username = UsernameComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(username))
            {
                createMatchWindow.expeledPlayerProfile = username;
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
            matchManagementClient.Close();
            Close();
        }
    }
}