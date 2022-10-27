using Parlis.Client.Services;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class CreateMatchWindow : Window, IMatchManagementCallback
    {
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        private readonly MatchManagementClient matchManagementClient;
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public CreateMatchWindow()
        {
            InitializeComponent();
            usernames = new TextBlock[] { FirstUsernameTextBox, SecondUsernameTextBox, ThirdUsernameTextBox, FourthUsernameTextBox };
            profilePictures = new Image[] { FirstProfilePicture, SecondProfilePicture, ThirdProfilePicture, FourthProfilePicture };
            var instanceContext = new InstanceContext(this);
            matchManagementClient = new MatchManagementClient(instanceContext);
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile, int code)
        {
            this.playerProfile = playerProfile;
            this.code = code;
            CodeLabel.Content = code + ".";
            try
            { 
                matchManagementClient.Connect(playerProfile.Username, this.code);
            } 
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void ExpelPlayerMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var expelPlayerWindow = new ExpelPlayerWindow();
            expelPlayerWindow.ShowDialog();
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            sendRealTimeMessageWindow.ShowDialog();
        }

        private void SendInvitationButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    if (playerProfileManagementClient.CheckPlayerProfileExistence(username))
                    {
                        SendMail(username);
                        playerProfileManagementClient.Close();
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
            else
            {
                MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                    Properties.Resources.EMPTY_FIELDS_WINDOW_TITLE);
            }
        }

        private void SendMail(string username)
        {
            string title = Properties.Resources.JOIN_MATCH_WINDOW_TITLE;
            string message = Properties.Resources.CODE_EMAIL_ADDRESS_LABEL;
            if (!playerProfileManagementClient.SendMail(username, title, message, code))
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void StartMatchButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                matchManagementClient.Disconnect(playerProfile.Username, code);
                matchManagementClient.Close();
                GoToMainMenu();
            } 
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        public void SendPlayerProfiles(string[] playerProfiles)
        {
            ConfigurePlayerProfiles(playerProfiles);
        }

        public void ConfigurePlayerProfiles(string[] playerProfiles)
        {
            for (int playerProfile = 0; playerProfile < playerProfiles.Length; playerProfile++)
            {
                string username = playerProfiles[playerProfile];
                usernames[playerProfile].Text = username;
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/ProfilePictures/" + username + ".jpg";
                try
                {
                    profilePictures[playerProfile].Source = new BitmapImage(new Uri(profilePicturePath));
                } catch (IOException)
                {
                    profilePictures[playerProfile].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
                }
            }
        }
    }
}