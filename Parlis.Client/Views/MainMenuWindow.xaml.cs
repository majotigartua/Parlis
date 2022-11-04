using Parlis.Client.Resources;
using Parlis.Client.Services;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class MainMenuWindow : Window, IMatchManagementCallback
    {
        private readonly MatchManagementClient matchManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public MainMenuWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            matchManagementClient = new MatchManagementClient(new InstanceContext(this));
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void CreateMatchButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            string username = playerProfile.Username;
            code = Utilities.GenerateRandomCode();
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
            string username = playerProfile.Username;
            if (!playerProfiles.Contains(username))
            {
                try
                {
                    GoToCreateMatch();
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_CONNECTED_WINDOW_TITLE
                    + " "
                    + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
            }
        }

        private void GoToCreateMatch()
        {
            var createMatchWindow = new CreateMatchWindow();
            createMatchWindow.ConfigureWindow(playerProfile, code);
            Close();
            createMatchWindow.Show();
        }

        private void JoinMatchButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var joinMatchWindow = new JoinMatchWindow();
            joinMatchWindow.ConfigureWindow(playerProfile);
            Close();
            joinMatchWindow.Show();
        }

        private void SettingsMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var gameConfigurationWindow = new GameConfigurationWindow();
            gameConfigurationWindow.ConfigureWindow(playerProfile);
            Close();
            gameConfigurationWindow.Show();
        }

        private void PlayerProfileMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var editPlayerProfileWindow = new EditPlayerProfileWindow();
            editPlayerProfileWindow.ConfigureWindow(playerProfile);
            Close();
            editPlayerProfileWindow.Show();
        }

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        public void StarMatch()
        {
            throw new System.NotImplementedException();
        }
    }
}