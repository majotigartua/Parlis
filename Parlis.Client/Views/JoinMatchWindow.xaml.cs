﻿using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Parlis.Client.Views
{
    public partial class JoinMatchWindow : Window, IMatchManagementCallback
    {
        private readonly MatchManagementClient matchManagementClient;
        private PlayerProfile playerProfile;
        private int code;

        public JoinMatchWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            matchManagementClient = new MatchManagementClient(new InstanceContext(this));
        }

        public void ConfigureWindow(PlayerProfile playerProfile)
        {
            this.playerProfile = playerProfile;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            if (!string.IsNullOrWhiteSpace(CodeTextBox.Text))
            {
                try
                {
                    code = int.Parse(CodeTextBox.Text);
                    if (matchManagementClient.CheckMatchExistence(code))
                    {
                        string username = playerProfile.Username;
                        matchManagementClient.GetPlayerProfiles(username, code);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                            Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show(Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL,
                        Properties.Resources.INVALID_DATA_WINDOW_TITLE);
                }
                catch (CommunicationException)
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

        public void ExpelPlayerProfileFromMatch(string username)
        {
        }

        public void ReceivePlayerProfiles(string[] playerProfiles)
        {
            int numberOfPlayerProfiles = playerProfiles.Length;
            JoinMatch(playerProfiles, numberOfPlayerProfiles);
        }

        private void JoinMatch(string[] playerProfiles, int numberOfPlayerProfiles)
        {
            if (numberOfPlayerProfiles > Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH)
            {
                string username = playerProfile.Username;
                if (!playerProfiles.Contains(username))
                {
                    if (numberOfPlayerProfiles < Constants.NUMBER_OF_PLAYER_PROFILES_PER_MATCH)
                    {
                        GoToCreateMatch();
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.FULL_MATCH_WINDOW_TITLE);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.PLAYER_PROFILE_ALREADY_CONNECTED_WINDOW_TITLE
                        + " "
                        + Properties.Resources.CHECK_ENTERED_INFORMATION_LABEL);
                }
            }
            else
            {
                GoToCreateMatch();
            }
        }

        private void GoToCreateMatch()
        {
            var createMatchWindow = new CreateMatchWindow();
            createMatchWindow.ConfigureWindow(playerProfile, code);
            Close();
            createMatchWindow.Show();
        }

        public void StartMatch()
        {
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            matchManagementClient.Close();
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            Close();
            mainMenuWindow.Show();
        }
    }
}