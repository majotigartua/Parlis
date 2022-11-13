﻿using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class GameWindow : Window, IGameManagementCallback
    {
        private readonly BitmapImage DEFAULT_PROFILE_PICTURE = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        private readonly GameManagementClient gameManagementClient;
        private Dictionary<string, int> playerProfiles;
        private int numberOfPlayerProfiles;
        private PlayerProfile playerProfile;
        private int code;

        //BoardComponents
        private readonly BitmapImage DEFAULT_DICE = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative));
        public Random randomDiceResult;
        public int dice;
        private readonly String[] Dices;

        private ImageBrush spriteCircle;

        public GameWindow()
        {
            /* spriteCircle = new ImageBrush();
            spriteCircle.ImageSource = new BitmapImage(new Uri("C:/Users/Propietario/source/repos/Parlis/Parlis.Client/Resources/Images/YourTurn.png", UriKind.Absolute));
            test.Fill = spriteCircle;*/

            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { RedUsernameTextBox, BlueUsernameTextBox, GreenUsernameTextBox, YellowUsernameTextBox };
            profilePictures = new Image[] { RedProfilePicture, BlueProfilePicture, GreenProfilePicture, YellowProfilePicture };
            Dices = new String[] { "/Resources/Images/Dice1.png", "/Resources/Images/Dice2.png", "/Resources/Images/Dice3.png", "/Resources/Images/Dice4.png", "/Resources/Images/Dice5.png", "/Resources/Images/Dice6.png" };
            this.randomDiceResult = new Random();
            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
            playerProfiles = new Dictionary<string, int> { };
        }

        public void ThrowDice()
        {

            dice = randomDiceResult.Next(1, 7);
            SetDiceValue(dice);
        }

        private void SetDiceValue(int val0)
        {
            this.FirstDice.IsEnabled = true;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[val0 - 1], UriKind.Relative));
        }

        public void ConfigureWindow(PlayerProfile playerProfile, int code)
        {
            this.code = code;
            this.playerProfile = playerProfile;
            ConfigureData();
            try
            {
                gameManagementClient.ConnectToBoard(playerProfile.Username, code);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
        }

        private void ConfigureData()
        {
            numberOfPlayerProfiles = playerProfiles.Count;
            for (int playerProfile = 0; playerProfile < 4; playerProfile++)
            {
                usernames[playerProfile].Text = "";
                profilePictures[playerProfile].Source = DEFAULT_PROFILE_PICTURE;
            }
            this.FirstDice.Source = DEFAULT_DICE;
        }

        private void ConfigurePlayerProfiles(Dictionary<string, int> playerProfiles)
        {
            for (int playerProfile = 0; playerProfile < numberOfPlayerProfiles; playerProfile++)
            {
                string username = playerProfiles.ElementAt(playerProfile).Key;
                usernames[(playerProfiles.ElementAt(playerProfile).Value) - 1].Text = username;
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
                try
                {
                    profilePictures[(playerProfiles.ElementAt(playerProfile).Value) - 1].Source = new BitmapImage(new Uri(profilePicturePath));
                }
                catch (IOException)
                {
                    profilePictures[playerProfile].Source = DEFAULT_PROFILE_PICTURE;
                }

                if (playerProfiles.ElementAt(playerProfile).Value - 1 == 2)
                {
                    ShowNextTurn(playerProfiles.ElementAt(playerProfile).Value - 1);
                }

                Console.WriteLine("TURNPLAYER: (" + (playerProfile + 1) + ") player: (" + username + ") ColorTeam: (" + playerProfiles.ElementAt(playerProfile).Value + ")");
            }
        }

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var mainMenu = new MainMenuWindow();
            Close();
            mainMenu.Show();
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            string username = playerProfile.Username;
            sendRealTimeMessageWindow.ConfigureWindow(username, code);
            sendRealTimeMessageWindow.ShowDialog();
        }

        private void FirstDiceMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            gameManagementClient.SetDiceResult();
        }

        public void ReceiveMove(Coin coin)
        {
            throw new NotImplementedException();
        }

        public void ReceivePlayerProfilesForBoard(Dictionary<string, int> playerProfilesTurns)
        {
            this.playerProfiles = playerProfilesTurns;
            ConfigureData();
            ConfigurePlayerProfiles(this.playerProfiles);
            gameManagementClient.StartGame();
        }

        public void ShowDiceResult(int result)
        {
            this.FirstDice.IsEnabled = true;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[result - 1], UriKind.Relative));
        }

        public void ShowNextTurn(int turn)
        {
            Console.WriteLine(turn);
            switch (turn)
            {
                case 0:
                    Point point = new Point(0, 0);
                    //this.TranslatePoint(point, RingTurn);
                    // RingCanvas.SetTop((UIElement)RingTurn,15);
                    //RingCanvas.SetLeft((UIElement)RingTurn, 15);
                    Canvas.SetTop(test, 15);
                    break;
                case 1:
                    Point point1 = new Point(140, 0);
                    this.TranslatePoint(point1, RingTurn);
                    break;
                case 2:
                    Point point2 = new Point(0, 65);
                    this.TranslatePoint(point2, RingTurn);
                    break;
                case 3:
                    Point point3 = new Point(140, 65);
                    this.TranslatePoint(point3, RingTurn);
                    break;

            }
        }
    }
}