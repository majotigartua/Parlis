using Parlis.Client.Resources;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Parlis.Client.Views
{
    public partial class GameWindow : Window, Client.Services.IGameManagementCallback
    {
        private readonly BitmapImage DEFAULT_PROFILE_PICTURE = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
        private readonly BitmapImage FOCUSED_DICE = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        private readonly GameManagementClient gameManagementClient;
        private Dictionary<string, int> playerProfiles;
        private int numberOfPlayerProfiles;
        private PlayerProfile playerProfile;
        private int code;

        //BoardComponents
        private readonly BitmapImage DEFAULT_DICE = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative));
        private Random randomDiceResult;
        private readonly String[] Dices;
        private int TURN_PLAYER;
        public GameWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { RedUsernameTextBox, BlueUsernameTextBox, GreenUsernameTextBox, YellowUsernameTextBox };
            profilePictures = new Image[] { RedProfilePicture, BlueProfilePicture, GreenProfilePicture, YellowProfilePicture };
            Dices = new String[] { "/Resources/Images/Dice1.png", "/Resources/Images/Dice2.png", "/Resources/Images/Dice3.png", "/Resources/Images/Dice4.png", "/Resources/Images/Dice5.png", "/Resources/Images/Dice6.png" };
            this.randomDiceResult = new Random();
            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
            playerProfiles = new Dictionary<string, int> { };
            TURN_PLAYER = 0;
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
                //string username = playerProfiles[colorTeam[playerProfile]];
                string username = playerProfiles.ElementAt(playerProfile).Key;
                //usernames[colorTeam[playerProfile]].Text = username;
                usernames[(playerProfiles.ElementAt(playerProfile).Value)-1].Text = username;
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
                try
                {
                    //profilePictures[colorTeam[playerProfile]].Source = new BitmapImage(new Uri(profilePicturePath));
                    profilePictures[(playerProfiles.ElementAt(playerProfile).Value) - 1].Source = new BitmapImage(new Uri(profilePicturePath));
                }
                catch (IOException)
                {
                    profilePictures[(playerProfiles.ElementAt(playerProfile).Value) - 1].Source = DEFAULT_PROFILE_PICTURE;
                }
                Console.WriteLine("TURNPLAYER: ("+(playerProfile + 1)+ ") player: (" +username+ ") ColorTeam: (" + playerProfiles.ElementAt(playerProfile).Value + ")");
            }
            if (this.playerProfile.Username == playerProfiles.First().Key)
            {
                this.FirstDice.IsEnabled = true;
                this.FocusedDice.Source = FOCUSED_DICE;
                gameManagementClient.SetNextTurn(playerProfiles.First().Value);
            }
            else
            {
                this.FirstDice.IsEnabled = false;
                this.FocusedDice.Source = null;

            }
        }

        public void ReceivePlayerProfilesForBoard(Dictionary<string, int> playerProfilesTurns)
        {
            this.playerProfiles = playerProfilesTurns;
            ConfigureData();
            ConfigurePlayerProfiles(this.playerProfiles);
        }


        public void ShowDiceResult(int diceResult)
        {
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[diceResult - 1], UriKind.Relative));
            if (TURN_PLAYER == 3)
            {
                this.TURN_PLAYER = 0;
            }
            else
                this.TURN_PLAYER++;
            gameManagementClient.SetNextTurn(playerProfiles.ElementAt(TURN_PLAYER).Value);

        }

        public void ShowNextTurn(int turn)
        {
            if (this.playerProfile.Username == playerProfiles.ElementAt(turn - 1).Key)
            {
                this.FirstDice.IsEnabled = true;
                this.FocusedDice.Source = FOCUSED_DICE;
            }
            else
            {
                this.FirstDice.IsEnabled = false;
                this.FocusedDice.Source = null;
            }
            switch (turn-1)
            {
                case 0:
                    Canvas.SetTop(RingTurn, 3);
                    Canvas.SetLeft(RingTurn, 8);
                    break;
                case 1:
                    Canvas.SetTop(RingTurn, 3);
                    Canvas.SetLeft(RingTurn, 148);
                    break;
                case 2:
                    Canvas.SetTop(RingTurn, 68);
                    Canvas.SetLeft(RingTurn, 8);
                    break;
                case 3:
                    Canvas.SetTop(RingTurn, 68);
                    Canvas.SetLeft(RingTurn, 148);
                    break;

            }
        }







        public void ConnectToBoard(string username, int code)
        {
            throw new NotImplementedException();
        }

        public Task ConnectToBoardAsync(string username, int code)
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromBoard(string username)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectFromBoardAsync(string username)
        {
            throw new NotImplementedException();
        }

        public void SendMove(int result, Coin coin)
        {
            throw new NotImplementedException();
        }

        public Task SendMoveAsync(int result, Coin coin)
        {
            throw new NotImplementedException();
        }

        public void GetPlayerProfilesForBoard(string username, int code)
        {
            throw new NotImplementedException();
        }

        public Task GetPlayerProfilesForBoardAsync(string username, int code)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMove(Coin coin)
        {
            throw new NotImplementedException();
        }





        //Extras
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

    }
}