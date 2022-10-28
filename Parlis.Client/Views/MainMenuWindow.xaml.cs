using Parlis.Client.Resources;
using Parlis.Client.Services;
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
            var instaceContext = new InstanceContext(this);
            matchManagementClient = new MatchManagementClient(instaceContext);
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;

        }

        private void CreateMatchButtonClick(object sender, RoutedEventArgs e)
        {
            code = Utilities.GenerateRandomCode();
            try
            {
                matchManagementClient.CreateMatch(code);
                GoToCreateMatch();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
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
            var joinMatchWindow = new JoinMatchWindow();
            joinMatchWindow.ConfigureWindow(playerProfile);
            Close();
            joinMatchWindow.Show();
        }

        private void SettingsMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var gameConfigurationWindow = new GameConfigurationWindow();
            gameConfigurationWindow.ConfigureView(playerProfile);
            Close();
            gameConfigurationWindow.Show();
        }

        private void PlayerProfileMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var editPlayerProfileWindow = new EditPlayerProfileWindow();
            editPlayerProfileWindow.ConfigureWindow(playerProfile);
            Close();
            editPlayerProfileWindow.Show();
        }

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        public void SendPlayerProfiles(string[] playerProfiles)
        {
        }
    }
}