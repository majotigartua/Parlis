using Parlis.Client.Services;
using System;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class JoinMatchWindow : Window, IMatchManagementCallback
    {
        private readonly MatchManagementClient matchManagementClient;
        private PlayerProfile playerProfile;
        private string[] playerProfiles;
        private int code;

        public JoinMatchWindow()
        {
            InitializeComponent();
            var instanceContext = new InstanceContext(this);
            matchManagementClient = new MatchManagementClient(instanceContext);
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CodeTextBox.Text))
            {
                JoinMatch();
            }
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void JoinMatch()
        {
            code = int.Parse(CodeTextBox.Text);
            try
            {
                if (matchManagementClient.CheckMatchExistence(code))
                {
                    matchManagementClient.Connect(code);
                    if (playerProfiles == null || playerProfiles.Length < 4 )
                    {
                        GoToCreateMatchWindow();
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.FULL_MATCH_WINDOW_TITLE);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void GoToCreateMatchWindow()
        {
            var createMatchWindow = new CreateMatchWindow();
            createMatchWindow.ConfigureWindow(playerProfile, true, code);
            Close();
            createMatchWindow.Show();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        public void GetPlayerProfiles(string[] playerProfiles)
        {
            this.playerProfiles = playerProfiles;
        }
    }
}