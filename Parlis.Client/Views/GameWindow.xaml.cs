using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
    public partial class GameWindow : Window, IGameManagementCallback
    {
        private readonly BitmapImage DEFAULT_PROFILE_PICTURE = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
        private readonly BitmapImage FOCUSED_DICE = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
        private readonly TextBlock[] usernames;
        private readonly Image[] profilePictures;
        private readonly GameManagementClient gameManagementClient;
        private List<Coin> coinsPlaying;
        private int numberOfCoins;
        private PlayerProfile playerProfile;
        private int code;

        //BoardComponents
        private readonly BitmapImage DEFAULT_DICE = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative));
        private readonly String[] Dices;
        private readonly Image[] coinsImages;

        //Coin elements?

        //extra components boards
        private int TURN_PLAYER, DICE_VALUE;
        private bool REROLL, EATINGCOIN;
        public GameWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { RedUsernameTextBox, BlueUsernameTextBox, GreenUsernameTextBox, YellowUsernameTextBox };
            profilePictures = new Image[] { RedProfilePicture, BlueProfilePicture, GreenProfilePicture, YellowProfilePicture };
            Dices = new String[] { "/Resources/Images/Dice1.png", "/Resources/Images/Dice2.png", "/Resources/Images/Dice3.png", "/Resources/Images/Dice4.png", "/Resources/Images/Dice5.png", "/Resources/Images/Dice6.png", "/Resources/Images/EatingCoin.png" };
            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
            coinsPlaying = new List<Coin> { };
            coinsImages = new Image[] { RedCoin, BlueCoin, GreenCoin, YellowCoin };
            TURN_PLAYER = 0;
            DICE_VALUE = 0;
            REROLL = false;
            EATINGCOIN = false;
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
            numberOfCoins = coinsPlaying.Count;
            for (int coinPlace = 0; coinPlace < 4; coinPlace++)
            {
                usernames[coinPlace].Text = "";
                profilePictures[coinPlace].Source = DEFAULT_PROFILE_PICTURE;
            }
            this.FirstDice.Source = DEFAULT_DICE;
        }
        private void ConfigurePlayerProfiles(List<Coin> coins)
        {
            for (int coin = 0; coin < numberOfCoins; coin++)
            {
                string username = coins.ElementAt(coin).PlayerProfileUsername;
                usernames[coins.ElementAt(coin).ColorTeamValue].Text = username;
                var profilePicturePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "../../ProfilePictures/" + username + ".jpg";
                try
                {
                    profilePictures[(coins.ElementAt(coin).ColorTeamValue)].Source = new BitmapImage(new Uri(profilePicturePath));
                }
                catch (IOException)
                {
                    profilePictures[coins.ElementAt(coin).ColorTeamValue].Source = DEFAULT_PROFILE_PICTURE;
                }
            }
        }
        public void ReceiveCoinsForBoard(Coin[] coins)
        {
            this.coinsPlaying = coins.ToList();
            ConfigureData();
            ConfigurePlayerProfiles(this.coinsPlaying);
            if (this.playerProfile.Username == coinsPlaying.First().PlayerProfileUsername)
            {
                this.FirstDice.IsEnabled = true;
                this.FocusedDice.Source = FOCUSED_DICE;
                gameManagementClient.SetNextTurn(coinsPlaying.First().ColorTeamValue);
            }
            else
            {
                this.FirstDice.IsEnabled = false;
                this.FocusedDice.Source = null;

            }
        }
        public void ShowDiceResult(int diceResult)
        {
            this.DICE_VALUE = diceResult;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[DICE_VALUE - 1], UriKind.Relative));
            HowMuchAndWhereToMove(TURN_PLAYER);
            if (!REROLL)
            {
                coinsPlaying.ElementAt(TURN_PLAYER).NumRolls = 0;
                this.REROLL = false;
                this.TURN_PLAYER++;
                if (this.TURN_PLAYER >= 4)
                {
                    this.TURN_PLAYER = 0;
                }
                gameManagementClient.SetNextTurn(coinsPlaying.ElementAt(TURN_PLAYER).ColorTeamValue);
            }
        }
        public void HowMuchAndWhereToMove(int turnPlayer)
        {
            int colorValueTeam = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            this.REROLL = false;
            coinsPlaying.ElementAt(turnPlayer).NumRolls++;

            #region test values
            //TEST ReRoll & HomeSlots
            //DICE_VALUE = 6;
            //TEST ReRoll adn HomeSlots

            //TEST IsFinalColorPathStarted
            //DICE_VALUE = 5;
            //TEST IsFinalColorPathStarted 

            //TEST EatCoin 1 Casilla Normal al limite & Share
            /*switch (colorValueTeam)
            {
                case 0:
                    DICE_VALUE = 2;
                    break;
                case 1:
                    DICE_VALUE = 4;
                    break;
                case 2:
                    DICE_VALUE = 4;
                    break;
                case 3:
                    //Para la 1er prueba y 2nd
                    //DICE_VALUE = 5;

                    //Para la 3er
                    DICE_VALUE = 1;
                    break;
            }*/
            //TEST EatCoin 1 Casilla Normal

            #endregion

            if (EATINGCOIN)
            {
                DICE_VALUE = 20;
                EATINGCOIN = false;
                coinsPlaying.ElementAt(turnPlayer).NumRolls--;
            }

            if (coinsPlaying.ElementAt(turnPlayer).NumRolls == 3)
            {
                GoToHomeSlot(turnPlayer);
            }
            else
            {
                if (DICE_VALUE == 6)
                {
                    this.REROLL = true;
                }
                coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot + DICE_VALUE;
                if (coinsPlaying.ElementAt(turnPlayer).AtFinalRow)
                {
                    MoveInFInalColorPath(turnPlayer);
                }
                else if (coinsPlaying.ElementAt(turnPlayer).AtSlot > 67)
                {
                    coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - 68;
                    coinsPlaying.ElementAt(turnPlayer).FisrtLeap = true;
                    if (colorValueTeam == 3)
                    {
                        coinsPlaying.ElementAt(turnPlayer).AtFinalRow = true;
                        MoveInFInalColorPath(turnPlayer);
                    }
                    else {
                        CheckForCoinsAtSameSlot(turnPlayer);
                    }
                }
                else if (AbleToStartFinalColorPath(turnPlayer))
                {
                    MoveInFInalColorPath(turnPlayer);
                }
                else 
                {
                    CheckForCoinsAtSameSlot(turnPlayer);
                }
            }

        }
        public bool AbleToStartFinalColorPath(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            bool ableToStart = false;
            if (coinsPlaying.ElementAt(turnPlayer).FisrtLeap && ((coinsPlaying.ElementAt(turnPlayer).AtSlot > Constants.INITIAL_COLOR_PATH_SLOT[colorTeamValue]) && (coinsPlaying.ElementAt(turnPlayer).AtSlot < (Constants.INITIAL_COLOR_PATH_SLOT[colorTeamValue] + 21))))
            {
                coinsPlaying.ElementAt(turnPlayer).AtFinalRow = true;
                coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - (Constants.INITIAL_COLOR_PATH_SLOT[colorTeamValue] + 1);
                ableToStart = true;
            }
            return ableToStart;
        }
        public void ShowNextTurn(int colorTeamValue)
        {
            if (this.playerProfile.Username == coinsPlaying.ElementAt(TURN_PLAYER).PlayerProfileUsername)
            {
                this.FirstDice.IsEnabled = true;
                this.FocusedDice.Source = FOCUSED_DICE;
            }
            else
            {
                this.FirstDice.IsEnabled = false;
                this.FocusedDice.Source = null;
            }
            switch (colorTeamValue)
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
        public void ShowCoinMoved(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            Canvas.SetTop(coinsImages[colorTeamValue], Constants.BOARD_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
            Canvas.SetLeft(coinsImages[colorTeamValue], Constants.BOARD_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
        }
        public void GoToHomeSlot(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            coinsPlaying.ElementAt(turnPlayer).AtFinalRow = false;
            coinsPlaying.ElementAt(turnPlayer).FisrtLeap = false;
            coinsPlaying.ElementAt(turnPlayer).AtSlot = Constants.INITIAL_SLOTS.ElementAt(colorTeamValue);
            Canvas.SetTop(coinsImages[colorTeamValue], Constants.HOME_SLOTS_CORDINATES[colorTeamValue].X);
            Canvas.SetLeft(coinsImages[colorTeamValue], Constants.HOME_SLOTS_CORDINATES[colorTeamValue].Y);
            coinsPlaying.ElementAt(turnPlayer).NumRolls = 0;
            this.REROLL = false;

        }
        public void MoveInFInalColorPath(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;

            if (coinsPlaying.ElementAt(turnPlayer).AtSlot != 7)
            {
                if (coinsPlaying.ElementAt(turnPlayer).AtSlot > 7)
                {
                    do
                    {
                        coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - (Constants.RED_PATH_SLOTS.Length);
                    } while (coinsPlaying.ElementAt(turnPlayer).AtSlot > 7);
                }
                switch (colorTeamValue)
                {
                    case 0:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.RED_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.RED_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 1:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.BLUE_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.BLUE_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 2:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.GREEN_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.GREEN_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 3:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.YELLOW_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.YELLOW_PATH_SLOTS[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                }
            }
            else if (coinsPlaying.ElementAt(turnPlayer).AtSlot == 7)
            {
                coinsPlaying.ElementAt(turnPlayer).IsWinner = true;
                GoToHomeSlot(turnPlayer);
            }
        }
        public void CheckForCoinsAtSameSlot(int turnPlayer)
        {
            bool ableToInvade = false;
            List<Coin> coinsToInvade = new List<Coin>(coinsPlaying);
            //INVADER DATA
            int turnInvaderPlayer = turnPlayer;
            int invaderPlayerSlotPosition = coinsToInvade.ElementAt(turnInvaderPlayer).AtSlot;
            double invaderXValue = Constants.BOARD_SLOTS[invaderPlayerSlotPosition].X;
            double invaderYValue = Constants.BOARD_SLOTS[invaderPlayerSlotPosition].Y;

            //INVADED DATA
            int turnInvadedPlayer = -1;
            int invadedPlayerSlotPosition = 0;
            double invadedXValue;
            double invadedYValue;
            coinsToInvade.RemoveAt(turnInvaderPlayer);
            coinsToInvade.RemoveAll(x => x.AtFinalRow == true);

            for (int coin = 0; coin < coinsToInvade.Count; coin++)
            {
                invadedPlayerSlotPosition = coinsToInvade.ElementAt(coin).AtSlot;
                invadedXValue = Constants.BOARD_SLOTS[invadedPlayerSlotPosition].X;
                invadedYValue = Constants.BOARD_SLOTS[invadedPlayerSlotPosition].Y;
                if ((invaderXValue == invadedXValue) && (invaderYValue == invadedYValue))
                {
                    turnInvadedPlayer = coinsPlaying.FindIndex(x => x.ColorTeamValue == coinsToInvade[coin].ColorTeamValue);
                    ableToInvade = true;
                    break;
                }
            }

            if (!coinsPlaying.ElementAt(turnInvaderPlayer).AtFinalRow)
            {
                if (ableToInvade)
                {
                    if (AtSafeSlot(invadedPlayerSlotPosition) == true)
                    {
                        ShareSlot(turnInvaderPlayer, turnInvadedPlayer);
                    }
                    else
                    {
                        EatCoin(turnInvaderPlayer, turnInvadedPlayer);
                    }
                }
                else
                {
                    gameManagementClient.SetCoinToMove(turnPlayer);
                }
            }
        }
        public void EatCoin(int turnInvaderPlayer, int turnInvadedPlayer)
        {
            GoToHomeSlot(turnInvadedPlayer);
            this.EATINGCOIN = true;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[6], UriKind.Relative));
            HowMuchAndWhereToMove(turnInvaderPlayer);
        }
        public void ShareSlot(int turnInvaderPlayer, int turnInvadedPlayer)
        {
            int sharedSlot = coinsPlaying.ElementAt(turnInvadedPlayer).AtSlot;
            int colorTeamValueInvader = coinsPlaying.ElementAt(turnInvaderPlayer).ColorTeamValue;
            int colorTeamValueInvaded = coinsPlaying.ElementAt(turnInvadedPlayer).ColorTeamValue;
            switch (sharedSlot)
            {
                case 4:
                case 28:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y - 4));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y + 4));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X));
                    break;
                case 11:
                case 55:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X - 1));
                    break;
                case 16:
                case 50:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X - 3));
                    break;
                case 21:
                case 45:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X - 1));
                    break;
                case 33:
                case 67:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y - 5));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y + 5));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X));

                    break;
                case 38:
                case 62:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].Y - 7));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BOARD_SLOTS[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].Y + 1));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BOARD_SLOTS[sharedSlot].X));
                    break;
            }
        }
        public bool AtSafeSlot(int invadedPlayerSlotPosition)
        {
            bool isSafeSlot = false;
            for (int slot = 0; slot < Constants.SAFE_SLOTS.Length; slot++)
            {
                if (invadedPlayerSlotPosition == Constants.SAFE_SLOTS[slot])
                {
                    isSafeSlot = true;
                    break;
                }

            }
            return isSafeSlot;
        }



        #region iNTERFACES
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
        #endregion

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
            gameManagementClient.ThrowDice();
        }



    }
}