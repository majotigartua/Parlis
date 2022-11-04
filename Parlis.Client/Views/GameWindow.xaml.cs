using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Parlis.Client.Views
{
    public partial class GameWindow : Window, Client.Services.IGameManagementCallback
    {
        private readonly BitmapImage DEFAULT_PROFILE_PICTURE = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        private readonly GameManagementClient gameManagementClient;
        private string[] playerProfiles;
        private int numberOfPlayerProfiles;
        private PlayerProfile playerProfile;
        private int code;

        //BoardComponents
        private readonly BitmapImage DEFAULT_DICE = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative));
        public Random randomDiceResult;
        public int dice;
        private readonly String[] Dices;


        public GameWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { FirstUsernameTextBox, SecondUsernameTextBox, ThirdUsernameTextBox, FourthUsernameTextBox };
            profilePictures = new Image[] { FirstProfilePicture, SecondProfilePicture, ThirdProfilePicture, FourthProfilePicture };
            Dices = new String[] { "/Resources/Images/Dice1.png", "/Resources/Images/Dice2.png", "/Resources/Images/Dice3.png", "/Resources/Images/Dice4.png", "/Resources/Images/Dice5.png", "/Resources/Images/Dice6.png" };
            this.randomDiceResult = new Random();
            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
            playerProfiles = new string[] { };
        }

        public void ThrowDice()
        {

            dice= randomDiceResult.Next(1, 7);
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
            numberOfPlayerProfiles = playerProfiles.Length;
            for (int playerProfile = 0; playerProfile < 4; playerProfile++)
            {
                usernames[playerProfile].Text = "";
                profilePictures[playerProfile].Source = DEFAULT_PROFILE_PICTURE;
            }
            this.FirstDice.Source = DEFAULT_DICE;
        }

        private void ConfigurePlayerProfiles(string[] playerProfiles)
        {

            for (int playerProfile = 0; playerProfile < numberOfPlayerProfiles; playerProfile++)
            {
                string username = playerProfiles[playerProfile];
                usernames[playerProfile].Text = username;
                Console.WriteLine(username);
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
                try
                {
                    profilePictures[playerProfile].Source = new BitmapImage(new Uri(profilePicturePath));
                }
                catch (IOException)
                {
                    profilePictures[playerProfile].Source = DEFAULT_PROFILE_PICTURE;
                }
            }
        }

        public void ReceivePlayerProfilesForBoard(string[] playerProfiles)
        {
            this.playerProfiles = playerProfiles;
            ConfigureData();
            ConfigurePlayerProfiles(this.playerProfiles);
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
            ThrowDice();
        }
    }
}