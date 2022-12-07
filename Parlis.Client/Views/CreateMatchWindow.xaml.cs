﻿using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class CreateMatchWindow : Window, IMatchManagementCallback
    {
        private static readonly int NUMBER_OF_PLAYER_PROFILES_PER_MATCH = 4;
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        public readonly MatchManagementClient matchManagementClient;
        private readonly PlayerProfileManagementClient playerProfileManagementClient;
        private PlayerProfile playerProfile;
        private int code;
        private int numberOfPlayerProfiles;

        public CreateMatchWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { FirstUsernameTextBox, SecondUsernameTextBox, ThirdUsernameTextBox, FourthUsernameTextBox };
            profilePictures = new Image[] { FirstProfilePicture, SecondProfilePicture, ThirdProfilePicture, FourthProfilePicture };
            matchManagementClient = new MatchManagementClient(new InstanceContext(this));
            playerProfileManagementClient = new PlayerProfileManagementClient();
        }

        public void ConfigureWindow(PlayerProfile playerProfile, int code)
        {
            this.playerProfile = playerProfile;
            this.code = code;
            try
            {
                if (!matchManagementClient.CheckMatchExistence(code))
                {
                    matchManagementClient.CreateMatch(code);
                }
                matchManagementClient.ConnectToMatch(playerProfile.Username, code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void ConfigureData()
        {
            CodeLabel.Content = code + ".";
            ExpelPlayer.IsEnabled = numberOfPlayerProfiles > 1;
            StartMatchButton.IsEnabled = numberOfPlayerProfiles == NUMBER_OF_PLAYER_PROFILES_PER_MATCH;
            for (int playerProfile = 0; playerProfile < NUMBER_OF_PLAYER_PROFILES_PER_MATCH; playerProfile++)
            {
                usernames[playerProfile].Text = "";
                profilePictures[playerProfile].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
            }
        }

        public void ReceivePlayerProfiles(string[] playerProfiles)
        {
            numberOfPlayerProfiles = playerProfiles.Length;
            string username = playerProfile.Username;
            if (playerProfiles.Contains(username))
            {
                ConfigureData();
                ConfigurePlayerProfiles(playerProfiles);
            }
        }

        private void ConfigurePlayerProfiles(string[] playerProfiles)
        {
            for (int playerProfile = 0; playerProfile < numberOfPlayerProfiles; playerProfile++)
            {
                string username = playerProfiles[playerProfile];
                usernames[playerProfile].Text = username;
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
                try
                {
                    profilePictures[playerProfile].Source = new BitmapImage(new Uri(profilePicturePath));
                }
                catch (IOException)
                {
                    profilePictures[playerProfile].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
                }
            }
        }

        private void ExpelPlayerMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var expelPlayerWindow = new ExpelPlayerWindow();
            string username = playerProfile.Username;
            expelPlayerWindow.ConfigureWindow(this, username, code);
            expelPlayerWindow.Show();
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            string username = playerProfile.Username;
            sendRealTimeMessageWindow.ConfigureWindow(username, code);
            sendRealTimeMessageWindow.ShowDialog();
        }

        private void SendInvitationButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var username = UsernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                try
                {
                    if (playerProfileManagementClient.CheckPlayerProfileExistence(username))
                    {
                        SendMail(username);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    }
                    UsernameTextBox.Clear();
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
            if (playerProfileManagementClient.SendMail(username, title, message, code))
            {
                MessageBox.Show(Properties.Resources.CODE_SENT_WINDOW_TITLE);
            }
            else
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void StartMatchButtonClick(object sender, RoutedEventArgs e)
        {
            matchManagementClient.SetBoards();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            string username = playerProfile.Username;
            try
            {
                matchManagementClient.DisconnectFromMatch(username, code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }

        public void StarMatch()
        {
            var gameWindow = new GameWindow();
            gameWindow.ConfigureWindow(this, playerProfile, code);
            Close();
            gameWindow.Show();
        }
    }
}