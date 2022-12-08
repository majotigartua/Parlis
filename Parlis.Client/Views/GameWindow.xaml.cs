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
        private readonly TextBlock[] usernames;
        private readonly string[] Dices;
        private readonly string[] PlacesResult;
        private readonly Image[] Medals;
        private readonly Image[] coinsImages;
        private readonly Image[] profilePictures;
        private readonly GameManagementClient gameManagementClient;
        private CreateMatchWindow createMatchWindow;
        private PlayerProfile playerProfile;
        private List<Coin> coinsPlaying;
        private int code;
        private int numberOfCoins;
        private int turnCoin;
        private int diceValue;
        private bool reRoll;
        private bool eatCoin;
        private bool finishGame;

        public GameWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            usernames = new TextBlock[] { RedUsernameTextBox, BlueUsernameTextBox, GreenUsernameTextBox, YellowUsernameTextBox };
            profilePictures = new Image[] { RedProfilePicture, BlueProfilePicture, GreenProfilePicture, YellowProfilePicture };
            Dices = new string[] { "/Resources/Images/Dice1.png", "/Resources/Images/Dice2.png", "/Resources/Images/Dice3.png", "/Resources/Images/Dice4.png", "/Resources/Images/Dice5.png", "/Resources/Images/Dice6.png", "/Resources/Images/EatingCoin.png", "/Resources/Images/FinishDice.png" };

            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
            coinsPlaying = new List<Coin> { };
            coinsImages = new Image[] { RedCoin, BlueCoin, GreenCoin, YellowCoin };
            PlacesResult = new string[] { "/Resources/Images/1stPlace.png", "/Resources/Images/2ndPlace.png", "/Resources/Images/3rdPlace.png", "/Resources/Images/4thPlace.png"};
            Medals = new Image[] {RedPlace, BluePlace, GreenPlace, YellowPlace};
            turnCoin = 0;
            diceValue = 0;
            reRoll = false;
            eatCoin = false;
            finishGame = false;
        }

        public void ConfigureWindow(CreateMatchWindow createMatchWindow, PlayerProfile playerProfile, int code)
        {
            this.createMatchWindow = createMatchWindow;
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
            for (int coinPlace = 0; coinPlace < 4; coinPlace++)
            {
                usernames[coinPlace].Text = "";
                profilePictures[coinPlace].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
            }
            this.FirstDice.Source = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative)); ;
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
                    profilePictures[coins.ElementAt(coin).ColorTeamValue].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
                }
            }
        }
        public void ReceiveCoinsForBoard(Coin[] coins)
        {
            this.coinsPlaying = coins.ToList();
            numberOfCoins = coinsPlaying.Count;
            string username = playerProfile.Username;
            
            if (coinsPlaying.Contains(coinsPlaying.Find(x => x.PlayerProfileUsername == username)))
            {
                ConfigureData();
                ConfigurePlayerProfiles(this.coinsPlaying);
                if (this.playerProfile.Username == coinsPlaying.First().PlayerProfileUsername)
                {
                    this.FirstDice.IsEnabled = true;
                    this.FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
                    gameManagementClient.SetNextTurn();
                }
                else
                {
                    this.FirstDice.IsEnabled = false;
                    this.FocusedDice.Source = null;

                }
            }

        }
        public void ShowDiceResult(int diceResult)
        {
            this.diceValue = diceResult;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[diceValue - 1], UriKind.Relative));
            HowMuchAndWhereToMove(turnCoin);
            if (!reRoll)
            {
                coinsPlaying.ElementAt(turnCoin).NumRolls = 0;
                this.reRoll = false;
                this.turnCoin++;
                if (this.turnCoin >= 4)
                {
                    this.turnCoin = 0;
                }
                gameManagementClient.SetNextTurn();
            }
        }
        public void HowMuchAndWhereToMove(int turnPlayer)
        {
            int colorValueTeam = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            this.reRoll = false;
            coinsPlaying.ElementAt(turnPlayer).NumRolls++;

            #region test values
            //TEST ReRoll & HomeSlots
            //diceValue = 6;
            //TEST ReRoll adn HomeSlots

            //TEST IsFinalColorPathStarted
            //diceValue = 5;
            //TEST IsFinalColorPathStarted 

            //TEST EatCoin 1 Casilla Normal al limite & Share
            switch (colorValueTeam)
            {
                case 0:
                    diceValue = 2;
                    break;
                case 1:
                    diceValue = 4;
                    break;
                case 2:
                    diceValue = 4;
                    break;
                case 3:
                    //Para la 1er prueba y 2nd
                    //diceValue = 5;

                    //Para la 3er
                    diceValue = 1;
                    break;
            }
            //TEST EatCoin 1 Casilla Normal

            #endregion

            if (eatCoin)
            {
                diceValue = 20;
                eatCoin = false;
                coinsPlaying.ElementAt(turnPlayer).NumRolls--;
            }

            if (coinsPlaying.ElementAt(turnPlayer).NumRolls == 3)
            {
                GoToHomeSlot(turnPlayer);
            }
            else
            {
                if (diceValue == 6)
                {
                    this.reRoll = true;
                }
                coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot + diceValue;
                if (coinsPlaying.ElementAt(turnPlayer).AtFinalRow)
                {
                    MoveInFInalColorPath(turnPlayer);
                }
                else if (coinsPlaying.ElementAt(turnPlayer).AtSlot > 67)
                {
                    coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - 68;
                    coinsPlaying.ElementAt(turnPlayer).FirstLeap = true;
                    if (colorValueTeam == 3)
                    {
                        coinsPlaying.ElementAt(turnPlayer).AtFinalRow = true;
                        coinsPlaying.ElementAt(turnPlayer).Points = 64;
                        MoveInFInalColorPath(turnPlayer);
                    }
                    else {
                        coinsPlaying.ElementAt(turnPlayer).Points = coinsPlaying.ElementAt(turnPlayer).Points + diceValue;
                        CheckForCoinsAtSameSlot(turnPlayer);
                    }
                }
                else if (AbleToStartFinalColorPath(turnPlayer))
                {
                    MoveInFInalColorPath(turnPlayer);
                }
                else 
                {
                    coinsPlaying.ElementAt(turnPlayer).Points = coinsPlaying.ElementAt(turnPlayer).Points + diceValue;
                    CheckForCoinsAtSameSlot(turnPlayer);
                }
            }

        }
        public bool AbleToStartFinalColorPath(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            bool ableToStart = false;
            if (coinsPlaying.ElementAt(turnPlayer).FirstLeap && ((coinsPlaying.ElementAt(turnPlayer).AtSlot > Constants.InitialColorPathSlot[colorTeamValue]) && (coinsPlaying.ElementAt(turnPlayer).AtSlot < (Constants.InitialColorPathSlot[colorTeamValue] + 21))))
            {
                coinsPlaying.ElementAt(turnPlayer).AtFinalRow = true;
                coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - (Constants.InitialColorPathSlot[colorTeamValue] + 1);
                coinsPlaying.ElementAt(turnPlayer).Points = 64;
                ableToStart = true;
            }
            return ableToStart;
        }
        public void ShowNextTurn()
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnCoin).ColorTeamValue;
            if (!finishGame)
            {
                Utilities.PlayGameSound(Constants.NEXT_TURN_CODE);
                Console.WriteLine("1- ShowNextTurn turnCoin=" + turnCoin + " colorTeamValue=" + colorTeamValue);
                Console.WriteLine("1.1- IsPlaying=" + (coinsPlaying.ElementAt(turnCoin).IsPlaying));
                while (!coinsPlaying.ElementAt(turnCoin).IsPlaying)
                {
                    turnCoin++;
                    Console.WriteLine("1.2- IsPlaying-turnCoin=" + turnCoin);
                    if (this.turnCoin >= 4)
                    {
                        this.turnCoin = 0;
                        Console.WriteLine("1.3- IsPlaying-turnCoin=" + turnCoin + " colorTeamValue=" + colorTeamValue);
                    }
                }
                Console.WriteLine("1.4- turnCoin=" + turnCoin);
                if (this.playerProfile.Username == coinsPlaying.ElementAt(turnCoin).PlayerProfileUsername)
                {
                    this.FirstDice.IsEnabled = true;
                    this.FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative)); ;
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
        }
        public void MoveInNormalPath(int turnPlayer)
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            Canvas.SetTop(coinsImages[colorTeamValue], Constants.BoardSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
            Canvas.SetLeft(coinsImages[colorTeamValue], Constants.BoardSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
        }
        public void GoToHomeSlot(int turnPlayer)
        {
            Utilities.PlayGameSound(Constants.GO_TO_HOME_SLOT_CODE);
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            coinsPlaying.ElementAt(turnPlayer).AtFinalRow = false;
            coinsPlaying.ElementAt(turnPlayer).FirstLeap = false;
            coinsPlaying.ElementAt(turnPlayer).AtSlot = Constants.InitialSlots.ElementAt(colorTeamValue);
            Canvas.SetTop(coinsImages[colorTeamValue], Constants.HomeSlotCordinates[colorTeamValue].X);
            Canvas.SetLeft(coinsImages[colorTeamValue], Constants.HomeSlotCordinates[colorTeamValue].Y);
            coinsPlaying.ElementAt(turnPlayer).NumRolls = 0;
            coinsPlaying.ElementAt(turnPlayer).Points = 0;
            this.reRoll = false;

        }
        public void MoveInFInalColorPath(int turnPlayer)
        {
            Utilities.PlayGameSound(Constants.COLOR_PATH_CODE);
            int colorTeamValue = coinsPlaying.ElementAt(turnPlayer).ColorTeamValue;
            if (coinsPlaying.ElementAt(turnPlayer).AtSlot != 7)
            {
                if (coinsPlaying.ElementAt(turnPlayer).AtSlot > 7)
                {
                    do
                    {
                        coinsPlaying.ElementAt(turnPlayer).AtSlot = coinsPlaying.ElementAt(turnPlayer).AtSlot - (Constants.RedPathSlots.Length);
                    } while (coinsPlaying.ElementAt(turnPlayer).AtSlot > 7);
                }
                switch (colorTeamValue)
                {
                    case 0:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.RedPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.RedPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 1:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.BluePathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.BluePathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 2:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.GreenPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.GreenPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                    case 3:
                        Canvas.SetTop(coinsImages[colorTeamValue], Constants.YellowPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].X);
                        Canvas.SetLeft(coinsImages[colorTeamValue], Constants.YellowPathSlots[coinsPlaying.ElementAt(turnPlayer).AtSlot].Y);
                        break;
                }
            }
            else if (coinsPlaying.ElementAt(turnPlayer).AtSlot == 7)
            {
                Utilities.PlayGameSound(Constants.WINNER_CODE);
                coinsPlaying.ElementAt(turnPlayer).IsWinner = true;
                WinnerPlayer();
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
            double invaderXValue = Constants.BoardSlots[invaderPlayerSlotPosition].X;
            double invaderYValue = Constants.BoardSlots[invaderPlayerSlotPosition].Y;

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
                invadedXValue = Constants.BoardSlots[invadedPlayerSlotPosition].X;
                invadedYValue = Constants.BoardSlots[invadedPlayerSlotPosition].Y;
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
        public bool AtSafeSlot(int invadedPlayerSlotPosition)
        {
            bool isSafeSlot = false;
            for (int slot = 0; slot < Constants.SafeSlots.Length; slot++)
            {
                if (invadedPlayerSlotPosition == Constants.SafeSlots[slot])
                {
                    isSafeSlot = true;
                    break;
                }

            }
            return isSafeSlot;
        }
        public void EatCoin(int turnInvaderPlayer, int turnInvadedPlayer)
        {
            Utilities.PlayGameSound(Constants.EAT_COIN_CODE);
            GoToHomeSlot(turnInvadedPlayer);
            this.eatCoin = true;
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[6], UriKind.Relative));
            HowMuchAndWhereToMove(turnInvaderPlayer);
        }
        public void ShareSlot(int turnInvaderPlayer, int turnInvadedPlayer)
        {
            Utilities.PlayGameSound(Constants.SHARE_SLOT_CODE);
            int sharedSlot = coinsPlaying.ElementAt(turnInvadedPlayer).AtSlot;
            int colorTeamValueInvader = coinsPlaying.ElementAt(turnInvaderPlayer).ColorTeamValue;
            int colorTeamValueInvaded = coinsPlaying.ElementAt(turnInvadedPlayer).ColorTeamValue;
            switch (sharedSlot)
            {
                case 4:
                case 28:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y - 4));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y + 4));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));
                    break;
                case 11:
                case 55:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 1));
                    break;
                case 16:
                case 50:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 3));
                    break;
                case 21:
                case 45:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 1));
                    break;
                case 33:
                case 67:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y - 5));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y + 5));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));

                    break;
                case 38:
                case 62:
                    Canvas.SetLeft(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y - 7));
                    Canvas.SetTop(coinsImages[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X));
                    Canvas.SetLeft(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y + 1));
                    Canvas.SetTop(coinsImages[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));
                    break;
            }
        }
        public void ShowDisconnectedPlayer(string disconectedPlayerUsername)
        {
            int coinAt = coinsPlaying.FindIndex(x => x.PlayerProfileUsername == disconectedPlayerUsername);
            GoToHomeSlot(coinAt);
            coinsPlaying.ElementAt(coinAt).IsPlaying = false;
            profilePictures[coinAt].Source = new BitmapImage(new Uri("/Resources/Images/DisconectedPlayer.png", UriKind.Relative));
            usernames[coinAt].Text = "";
            if (turnCoin == coinAt) {
                gameManagementClient.SetNextTurn();
            }
           /* if (coinsPlaying.LongCount(x => x.IsPlaying == false) == 4)
            { 
            
            }*/
        }

        public void WinnerPlayer()
        {
            List<Coin> finalResultCoin = new List<Coin>(coinsPlaying);
            string winnerPlayerUsername = finalResultCoin.Find(x => x.IsWinner == true).PlayerProfileUsername;
            int winnerColorTeamValue = finalResultCoin.Find(x => x.IsWinner == true).ColorTeamValue;
            finalResultCoin.RemoveAll(x => x.IsWinner == true);
            int placement = 0;
            List<Coin> coinsAtNormalPath = new List<Coin>();
            Medals[winnerColorTeamValue].Source = new BitmapImage(new Uri(PlacesResult[placement], UriKind.Relative));
            placement++;
            finalResultCoin.RemoveAll(x => x.IsPlaying == false);
            coinsAtNormalPath.AddRange(finalResultCoin.FindAll(x => x.AtFinalRow == false));
            if (coinsAtNormalPath.Count > 0) 
            {
                coinsAtNormalPath.OrderBy(x => x.Points);
                finalResultCoin.RemoveAll(x => x.AtFinalRow == false);
            }

            if (finalResultCoin.Count > 0)
            {
                finalResultCoin.OrderBy(x => x.AtSlot);
            }
            finalResultCoin.AddRange(coinsAtNormalPath);

            foreach (var coin in finalResultCoin)
            {
                Medals[coin.ColorTeamValue].Source = new BitmapImage(new Uri(PlacesResult[placement], UriKind.Relative));
                placement++;
            }
            this.FirstDice.Source = new BitmapImage(new Uri(Dices[7], UriKind.Relative));
            this.FirstDice.IsEnabled = false;
            this.FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative)); ;
            this.RingTurn.Source = null;
            finishGame = true;        }

        public void RegisterMatch(PlayerProfile playerProfile)
        {
            int winnerTurn = coinsPlaying.FindIndex(x => x.IsWinner == true);
            if (winnerTurn >= 0)
            {
                string username = playerProfile.Username;
                string winnerPlayer = coinsPlaying.ElementAt(winnerTurn).PlayerProfileUsername;
                if (username == winnerPlayer)
                {
                    var match = new Match()
                    {
                        Date = DateTime.Now,
                        PlayerProfileUsername = username,
                    };
                    if (!gameManagementClient.RegisterMatch(match))
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.NO_DATABASE_CONNECTION_WINDOW_TITLE);
                    }
                }
            }
        }

        public void GoToMainMenu()
        {
            gameManagementClient.Close();
            createMatchWindow.matchManagementClient.Close();
            var mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.ConfigureWindow(playerProfile);
            this.Close();
            mainMenuWindow.Show();

        }



        //Extras

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayGameSound(Constants.BUMMER_CODE);
            string username = playerProfile.Username;
            RegisterMatch(playerProfile);
            try
            {
                gameManagementClient.DisconnectFromBoard(username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            GoToMainMenu();

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
            if (!finishGame)
            {
                Utilities.PlayGameSound(Constants.THROW_DICE_CODE);
                gameManagementClient.ThrowDice();
                Utilities.PlayGameSound(Constants.MOVE_COIN_CODE);
                Utilities.PlayGameSound(Constants.NEXT_TURN_CODE);
            }
            else
            {
                Utilities.PlayGameSound(Constants.WINNER_CODE);
            }
        }
    }
}