using Parlis.Client.Resources;
using Parlis.Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Parlis.Client.Views
{
    public partial class GameWindow : Window, IGameManagementCallback
    {
        private readonly Image[] profilePictures;
        private readonly TextBlock[] usernames;
        private readonly Image[] coins;
        private readonly Image[] medals;
        private readonly string[] dices;
        private readonly string[] places;
        private List<Coin> coinsPlaying;
        private int turnCoin;
        private int diceValue;
        private bool reRoll;
        private bool eatCoin;
        private bool finishGame;
        private readonly GameManagementClient gameManagementClient;
        private CreateMatchWindow createMatchWindow;
        private PlayerProfile playerProfile;
        private int code;
        private int numberOfCoins;

        public GameWindow()
        {
            InitializeComponent();
            Utilities.PlayMusic();
            profilePictures = new Image[] { 
                RedProfilePicture,
                BlueProfilePicture,
                GreenProfilePicture,
                YellowProfilePicture
            };
            usernames = new TextBlock[] { 
                RedUsernameTextBox, 
                BlueUsernameTextBox,
                GreenUsernameTextBox,
                YellowUsernameTextBox 
            };
            coins = new Image[] {
                RedCoin, 
                BlueCoin, 
                GreenCoin, 
                YellowCoin 
            };
            medals = new Image[] {
                RedPlace,
                BluePlace,
                GreenPlace,
                YellowPlace
            };
            dices = new string[] {
                "/Resources/Images/Dice1.png",
                "/Resources/Images/Dice2.png",
                "/Resources/Images/Dice3.png",
                "/Resources/Images/Dice4.png",
                "/Resources/Images/Dice5.png",
                "/Resources/Images/Dice6.png",
                "/Resources/Images/EatingCoin.png",
                "/Resources/Images/FinishDice.png"
            };
            places = new string[] {
                "/Resources/Images/1stPlace.png",
                "/Resources/Images/2ndPlace.png",
                "/Resources/Images/3rdPlace.png",
                "/Resources/Images/4thPlace.png"
            };
            coinsPlaying = new List<Coin> { };
            turnCoin = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            diceValue = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            reRoll = false;
            eatCoin = false;
            finishGame = false;
            var instanceContext = new InstanceContext(this);
            gameManagementClient = new GameManagementClient(instanceContext);
        }

        public void ConfigureWindow(CreateMatchWindow createMatchWindow, PlayerProfile playerProfile, int code)
        {
            this.createMatchWindow = createMatchWindow;
            this.playerProfile = playerProfile;
            this.code = code;
            ConfigureData();
            try
            {
                gameManagementClient.ConnectToBoard(playerProfile.Username, code);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                GoToMainMenu();
            }
        }

        private void ConfigureData()
        {
            for (int playerProfile = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH; playerProfile < Constants.NUMBER_OF_PLAYER_PROFILES_PER_MATCH; playerProfile++)
            {
                usernames[playerProfile].Text = "";
                profilePictures[playerProfile].Source = new BitmapImage(new Uri("/Resources/Images/DefaultProfilePicture.png", UriKind.Relative));
            }
            FirstDice.Source = new BitmapImage(new Uri("/Resources/Images/Dice.png", UriKind.Relative)); ;
        }

        public void MoveInNormalPath(int playerProfileTurn)
        {
            int colorTeamValue = coinsPlaying.ElementAt(playerProfileTurn).ColorTeamValue;
            Canvas.SetTop(coins[colorTeamValue], Constants.BoardSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].X);
            Canvas.SetLeft(coins[colorTeamValue], Constants.BoardSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].Y);
        }

        public void ReceiveCoinsForBoard(Coin[] coins)
        {
            coinsPlaying = coins.ToList();
            numberOfCoins = coinsPlaying.Count;
            string username = playerProfile.Username;
            if (coinsPlaying.Contains(coinsPlaying.Find(coin => coin.PlayerProfileUsername.Equals(username))))
            {
                ConfigureData();
                ConfigurePlayerProfiles(coinsPlaying);
                if (coinsPlaying.First().PlayerProfileUsername.Equals(playerProfile.Username))
                {
                    FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
                    FirstDice.IsEnabled = true;
                    try
                    {
                        gameManagementClient.SetNextTurn();
                    }
                    catch (CommunicationException)
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                    }
                }
                else
                {
                    FocusedDice.Source = null;
                    FirstDice.IsEnabled = false;
                }
            }
        }

        private void ConfigurePlayerProfiles(List<Coin> coins)
        {
            for (int coin = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH; coin < numberOfCoins; coin++)
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

        public void ShowDiceResult(int diceResult)
        {
            diceValue = diceResult;
            FirstDice.Source = new BitmapImage(new Uri(dices[diceValue - 1], UriKind.Relative));
            HowMuchAndWhereToMove(turnCoin);
            if (!reRoll)
            {
                coinsPlaying.ElementAt(turnCoin).NumRolls = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
                turnCoin++;
                if (turnCoin >= Constants.NUMBER_OF_PLAYER_PROFILES_PER_MATCH)
                {
                    turnCoin = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
                }
                reRoll = false;
                try
                {
                    gameManagementClient.SetNextTurn();
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                    GoToMainMenu();
                }
            }
        }

        public void HowMuchAndWhereToMove(int playerProfileTurn)
        {
            int colorValueTeam = coinsPlaying.ElementAt(playerProfileTurn).ColorTeamValue;
            coinsPlaying.ElementAt(playerProfileTurn).NumRolls++;
            reRoll = false;
            if (eatCoin)
            {
                coinsPlaying.ElementAt(playerProfileTurn).NumRolls--;
                diceValue = Constants.NUMBER_OF_SLOTS_PER_EATED_COIN;
                eatCoin = false;
            }
            if (coinsPlaying.ElementAt(playerProfileTurn).NumRolls.Equals(Constants.MAXIUM_REROLLS_PER_PLAYER_PROFILE))
            {
                GoToHomeSlot(playerProfileTurn);
            }
            else
            {
                if (diceValue.Equals(Constants.NUMBER_OF_DICE_RESULT_TO_REROLL))
                {
                    reRoll = true;
                }
                coinsPlaying.ElementAt(playerProfileTurn).AtSlot = coinsPlaying.ElementAt(playerProfileTurn).AtSlot + diceValue;
                if (coinsPlaying.ElementAt(playerProfileTurn).AtFinalRow)
                {
                    MoveInFinalColorPath(playerProfileTurn);
                }
                else if (coinsPlaying.ElementAt(playerProfileTurn).AtSlot > Constants.MAXIUM_SLOTS_PER_LEAP)
                {
                    coinsPlaying.ElementAt(playerProfileTurn).AtSlot = coinsPlaying.ElementAt(playerProfileTurn).AtSlot - (Constants.MAXIUM_SLOTS_PER_LEAP + 1);
                    coinsPlaying.ElementAt(playerProfileTurn).FirstLeap = true;
                    if (colorValueTeam.Equals(Constants.MAXIUM_REROLLS_PER_PLAYER_PROFILE))
                    {
                        coinsPlaying.ElementAt(playerProfileTurn).AtFinalRow = true;
                        coinsPlaying.ElementAt(playerProfileTurn).Points = Constants.NUMBER_OF_POINTS_AT_FINAL_ROW;
                        MoveInFinalColorPath(playerProfileTurn);
                    }
                    else {
                        coinsPlaying.ElementAt(playerProfileTurn).Points = coinsPlaying.ElementAt(playerProfileTurn).Points + diceValue;
                        CheckForCoinsAtSameSlot(playerProfileTurn);
                    }
                }
                else if (AbleToStartFinalColorPath(playerProfileTurn))
                {
                    MoveInFinalColorPath(playerProfileTurn);
                }
                else 
                {
                    coinsPlaying.ElementAt(playerProfileTurn).Points = coinsPlaying.ElementAt(playerProfileTurn).Points + diceValue;
                    CheckForCoinsAtSameSlot(playerProfileTurn);
                }
            }
        }

        public void CheckForCoinsAtSameSlot(int playerProfileTurn)
        {
            List<Coin> coinsToInvade = new List<Coin>(coinsPlaying);
            int turnInvaderPlayer = playerProfileTurn;
            int turnInvadedPlayer = Constants.NUMBER_OF_TURN_PER_INVADED_PLAYER;
            int invaderPlayerSlotPosition = coinsToInvade.ElementAt(turnInvaderPlayer).AtSlot;
            int invadedPlayerSlotPosition = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            double invaderXValue = Constants.BoardSlots[invaderPlayerSlotPosition].X;
            double invaderYValue = Constants.BoardSlots[invaderPlayerSlotPosition].Y;
            coinsToInvade.RemoveAt(turnInvaderPlayer);
            coinsToInvade.RemoveAll(x => x.AtFinalRow == true);
            bool ableToInvade = false;
            for (int coin = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH; coin < coinsToInvade.Count; coin++)
            {
                invadedPlayerSlotPosition = coinsToInvade.ElementAt(coin).AtSlot;
                double invadedXValue = Constants.BoardSlots[invadedPlayerSlotPosition].X;
                double invadedYValue = Constants.BoardSlots[invadedPlayerSlotPosition].Y;
                if (invaderXValue.Equals(invadedXValue) && invaderYValue.Equals(invadedYValue))
                {
                    turnInvadedPlayer = coinsPlaying.FindIndex(coinPlaying => coinPlaying.ColorTeamValue.Equals(coinsToInvade[coin].ColorTeamValue));
                    ableToInvade = true;
                    break;
                }
            }
            if (!coinsPlaying.ElementAt(turnInvaderPlayer).AtFinalRow)
            {
                if (ableToInvade)
                {
                    if (AtSafeSlot(invadedPlayerSlotPosition))
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
                    try
                    {
                        gameManagementClient.SetCoinToMove(playerProfileTurn);
                    }
                    catch (CommunicationException)
                    {
                        MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                            Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                        GoToMainMenu();
                    }
                }
            }
        }

        public bool AtSafeSlot(int invadedPlayerSlotPosition)
        {
            bool isSafeSlot = false;
            for (int slot = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH; slot < Constants.SafeSlots.Length; slot++)
            {
                if (invadedPlayerSlotPosition == Constants.SafeSlots[slot])
                {
                    isSafeSlot = true;
                    break;
                }

            }
            return isSafeSlot;
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
                    Canvas.SetLeft(coins[colorTeamValueInvader], Constants.BoardSlots[sharedSlot].Y - 4);
                    Canvas.SetTop(coins[colorTeamValueInvader], Constants.BoardSlots[sharedSlot].X);
                    Canvas.SetLeft(coins[colorTeamValueInvaded], Constants.BoardSlots[sharedSlot].Y + 4);
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));
                    break;
                case 11:
                case 55:
                    Canvas.SetLeft(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 1));
                    break;
                case 16:
                case 50:
                    Canvas.SetLeft(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 3));
                    break;
                case 21:
                case 45:
                    Canvas.SetLeft(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X + 3));
                    Canvas.SetLeft(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y));
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X - 1));
                    break;
                case 33:
                case 67:
                    Canvas.SetLeft(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y - 5));
                    Canvas.SetTop(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X));
                    Canvas.SetLeft(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y + 5));
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));

                    break;
                case 38:
                case 62:
                    Canvas.SetLeft(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].Y - 7));
                    Canvas.SetTop(coins[colorTeamValueInvader], (Constants.BoardSlots[sharedSlot].X));
                    Canvas.SetLeft(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].Y + 1));
                    Canvas.SetTop(coins[colorTeamValueInvaded], (Constants.BoardSlots[sharedSlot].X));
                    break;
            }
        }

        public void EatCoin(int turnInvaderPlayer, int turnInvadedPlayer)
        {
            Utilities.PlayGameSound(Constants.EAT_COIN_CODE);
            GoToHomeSlot(turnInvadedPlayer);
            FirstDice.Source = new BitmapImage(new Uri(dices[Constants.NUMBER_OF_DICE_RESULT_TO_REROLL], UriKind.Relative));
            eatCoin = true;
            HowMuchAndWhereToMove(turnInvaderPlayer);
        }

        public void MoveInFinalColorPath(int playerProfileTurn)
        {
            Utilities.PlayGameSound(Constants.COLOR_PATH_CODE);
            int colorTeamValue = coinsPlaying.ElementAt(playerProfileTurn).ColorTeamValue;
            if (!coinsPlaying.ElementAt(playerProfileTurn).AtSlot.Equals(Constants.MAXIUM_SLOTS_AT_FINAL_ROW))
            {
                if (coinsPlaying.ElementAt(playerProfileTurn).AtSlot > Constants.MAXIUM_SLOTS_AT_FINAL_ROW)
                {
                    do
                    {
                        coinsPlaying.ElementAt(playerProfileTurn).AtSlot = coinsPlaying.ElementAt(playerProfileTurn).AtSlot - Constants.RedPathSlots.Length;
                    }
                    while (coinsPlaying.ElementAt(playerProfileTurn).AtSlot > Constants.MAXIUM_SLOTS_AT_FINAL_ROW);
                }
                switch (colorTeamValue)
                {
                    case Constants.RED_COIN_CODE:
                        Canvas.SetTop(coins[colorTeamValue], Constants.RedPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].X);
                        Canvas.SetLeft(coins[colorTeamValue], Constants.RedPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].Y);
                        break;
                    case Constants.BLUE_COIN_CODE:
                        Canvas.SetTop(coins[colorTeamValue], Constants.BluePathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].X);
                        Canvas.SetLeft(coins[colorTeamValue], Constants.BluePathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].Y);
                        break;
                    case Constants.GREEN_COIN_CODE:
                        Canvas.SetTop(coins[colorTeamValue], Constants.GreenPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].X);
                        Canvas.SetLeft(coins[colorTeamValue], Constants.GreenPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].Y);
                        break;
                    case Constants.YELLOW_COIN_CODE:
                        Canvas.SetTop(coins[colorTeamValue], Constants.YellowPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].X);
                        Canvas.SetLeft(coins[colorTeamValue], Constants.YellowPathSlots[coinsPlaying.ElementAt(playerProfileTurn).AtSlot].Y);
                        break;
                }
            }
            else if (coinsPlaying.ElementAt(playerProfileTurn).AtSlot.Equals(Constants.MAXIUM_SLOTS_AT_FINAL_ROW))
            {
                Utilities.PlayGameSound(Constants.WINNER_CODE);
                coinsPlaying.ElementAt(playerProfileTurn).IsWinner = true;
                SetWinnerPlayer();
                GoToHomeSlot(playerProfileTurn);
            }
        }

        public void SetWinnerPlayer()
        {
            List<Coin> coinsAtNormalPath = new List<Coin>();
            List<Coin> finalResultCoins = new List<Coin>(coinsPlaying);
            string winner = finalResultCoins.Find(coin => coin.IsWinner).PlayerProfileUsername;
            int winnerColorTeamValue = finalResultCoins.Find(coin => coin.IsWinner).ColorTeamValue;
            int placement = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            medals[winnerColorTeamValue].Source = new BitmapImage(new Uri(places[placement], UriKind.Relative));
            coinsAtNormalPath.AddRange(finalResultCoins.FindAll(coin => !coin.AtFinalRow));
            finalResultCoins.RemoveAll(x => x.IsWinner);
            finalResultCoins.RemoveAll(x => !x.IsPlaying);
            if (coinsAtNormalPath.Count > Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH)
            {
                coinsAtNormalPath.OrderBy(coin => coin.Points);
                finalResultCoins.RemoveAll(coin => !coin.AtFinalRow);
            }
            if (finalResultCoins.Count > Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH)
            {
                finalResultCoins.OrderBy(coin => coin.AtSlot);
            }
            finalResultCoins.AddRange(coinsAtNormalPath);
            foreach (var coin in finalResultCoins)
            {
                placement++;
                medals[coin.ColorTeamValue].Source = new BitmapImage(new Uri(places[placement], UriKind.Relative));
            }
            FirstDice.Source = new BitmapImage(new Uri(dices[Constants.FINAL_DICE_VALUE], UriKind.Relative));
            FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
            FirstDice.IsEnabled = false;
            RingTurn.Source = null;
            finishGame = true;
        }

        public void GoToHomeSlot(int playerProfileTurn)
        {
            Utilities.PlayGameSound(Constants.GO_TO_HOME_SLOT_CODE);
            int colorTeamValue = coinsPlaying.ElementAt(playerProfileTurn).ColorTeamValue;
            coinsPlaying.ElementAt(playerProfileTurn).AtFinalRow = false;
            coinsPlaying.ElementAt(playerProfileTurn).FirstLeap = false;
            coinsPlaying.ElementAt(playerProfileTurn).AtSlot = Constants.InitialSlots.ElementAt(colorTeamValue);
            coinsPlaying.ElementAt(playerProfileTurn).NumRolls = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            coinsPlaying.ElementAt(playerProfileTurn).Points = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
            Canvas.SetTop(coins[colorTeamValue], Constants.HomeSlotCordinates[colorTeamValue].X);
            Canvas.SetLeft(coins[colorTeamValue], Constants.HomeSlotCordinates[colorTeamValue].Y);
            reRoll = false;
        }

        public bool AbleToStartFinalColorPath(int playerProfileTurn)
        {
            int colorTeamValue = coinsPlaying.ElementAt(playerProfileTurn).ColorTeamValue;
            bool ableToStart = false;
            if (coinsPlaying.ElementAt(playerProfileTurn).FirstLeap && coinsPlaying.ElementAt(playerProfileTurn).AtSlot > Constants.InitialColorPathSlot[colorTeamValue] && coinsPlaying.ElementAt(playerProfileTurn).AtSlot < (Constants.InitialColorPathSlot[colorTeamValue] + 21))
            {
                coinsPlaying.ElementAt(playerProfileTurn).AtFinalRow = true;
                coinsPlaying.ElementAt(playerProfileTurn).AtSlot = coinsPlaying.ElementAt(playerProfileTurn).AtSlot - (Constants.InitialColorPathSlot[colorTeamValue] + 1);
                coinsPlaying.ElementAt(playerProfileTurn).Points = Constants.NUMBER_OF_POINTS_AT_FINAL_ROW;
                ableToStart = true;
            }
            return ableToStart;
        }
        public void ShowDisconnectedPlayerProfile(string username)
        {
            int coinAt = coinsPlaying.FindIndex(coin => coin.PlayerProfileUsername.Equals(username));
            int colorTeamValue = coinsPlaying.ElementAt(coinAt).ColorTeamValue;
            profilePictures[colorTeamValue].Source = new BitmapImage(new Uri("/Resources/Images/DisconectedPlayer.png", UriKind.Relative));
            usernames[colorTeamValue].Text = "";
            if (turnCoin.Equals(coinAt))
            {
                turnCoin++;
                try
                {
                    gameManagementClient.SetNextTurn();
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                    GoToMainMenu();
                }
            }
            coinsPlaying.ElementAt(coinAt).IsPlaying = false;
            GoToHomeSlot(coinAt);
        }

        public void ShowNextTurn()
        {
            int colorTeamValue = coinsPlaying.ElementAt(turnCoin).ColorTeamValue;
            if (!finishGame)
            {
                Utilities.PlayGameSound(Constants.NEXT_TURN_CODE);
                while (!coinsPlaying.ElementAt(turnCoin).IsPlaying)
                {
                    turnCoin++;
                    if (turnCoin >= Constants.NUMBER_OF_PLAYER_PROFILES_PER_MATCH)
                    {
                        turnCoin = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH;
                    }
                }
                if (playerProfile.Username.Equals(coinsPlaying.ElementAt(turnCoin).PlayerProfileUsername))
                {
                    FocusedDice.Source = new BitmapImage(new Uri("/Resources/Images/FocusedDice.png", UriKind.Relative));
                    FirstDice.IsEnabled = true;
                }
                else
                {
                    FocusedDice.Source = null;
                    FirstDice.IsEnabled = false;
                }
                switch (colorTeamValue)
                {
                    case Constants.RED_COIN_CODE:
                        Canvas.SetTop(RingTurn, 3);
                        Canvas.SetLeft(RingTurn, 8);
                        break;
                    case Constants.BLUE_COIN_CODE:
                        Canvas.SetTop(RingTurn, 3);
                        Canvas.SetLeft(RingTurn, 148);
                        break;
                    case Constants.GREEN_COIN_CODE:
                        Canvas.SetTop(RingTurn, 68);
                        Canvas.SetLeft(RingTurn, 8);
                        break;
                    case Constants.YELLOW_COIN_CODE:
                        Canvas.SetTop(RingTurn, 68);
                        Canvas.SetLeft(RingTurn, 148);
                        break;
                }
            }
        }

        private void ExitMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayGameSound(Constants.BUMMER_CODE);
            string username = playerProfile.Username;
            try
            {
                RegisterMatch(username);
                gameManagementClient.DisconnectFromBoard(username);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                    Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
            }
            GoToMainMenu();
        }

        public void RegisterMatch(string username)
        {
            int winnerTurn = coinsPlaying.FindIndex(coin => coin.IsWinner);
            if (winnerTurn >= Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH)
            {
                string winner = coinsPlaying.ElementAt(winnerTurn).PlayerProfileUsername;
                if (username.Equals(winner))
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
            Close();
            mainMenuWindow.Show();
        }

        private void FirstDiceMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!finishGame)
            {
                Utilities.PlayGameSound(Constants.THROW_DICE_CODE);
                Utilities.PlayGameSound(Constants.MOVE_COIN_CODE);
                Utilities.PlayGameSound(Constants.NEXT_TURN_CODE);
                try
                {
                    gameManagementClient.ThrowDice();
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Properties.Resources.TRY_AGAIN_LATER_LABEL,
                        Properties.Resources.NO_SERVER_CONNECTION_WINDOW_TITLE);
                    GoToMainMenu();
                }
            }
            else
            {
                Utilities.PlayGameSound(Constants.WINNER_CODE);
            }
        }

        private void MessageBalloonMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Utilities.PlayButtonClickSound();
            var sendRealTimeMessageWindow = new SendRealTimeMessageWindow();
            string username = playerProfile.Username;
            sendRealTimeMessageWindow.ConfigureWindow(username, code);
            sendRealTimeMessageWindow.ShowDialog();
        }
    }
}