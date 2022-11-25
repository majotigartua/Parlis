using Parlis.Server.DataAccess;
using Parlis.Server.Service.Services;
using Player = Parlis.Server.Service.Data.Player;
using PlayerProfile = Parlis.Server.Service.Data.PlayerProfile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using Parlis.Server.Service.Data;

namespace Parlis.Server.BusinessLogic
{
    public partial class Service : IPlayerProfileManagement
    {
        public bool CheckPlayerExistence(string emailAddress)
        {
            using (ParlisContext context = new ParlisContext())
            {
                int numberOfPlayers = (from player in context.Players
                                       where player.EmailAddress.Equals(emailAddress)
                                       select player).Count();
                return numberOfPlayers > 0;
            }
        }

        public bool CheckPlayerProfileExistence(string username)
        {
            using (ParlisContext context = new ParlisContext())
            {
                int numberOfPlayerProfiles = (from playerProfile in context.PlayerProfiles
                                              where playerProfile.Username.Equals(username)
                                              select playerProfile).Count();
                return numberOfPlayerProfiles > 0;
            }
        }

        public bool DeletePlayer(string emailAddress)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var player = (from players in context.Players
                                  where players.EmailAddress.Equals(emailAddress)
                                  select players).First();
                    context.Players.Remove(player);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool DeletePlayerProfile(string username)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfile = (from playerProfiles in context.PlayerProfiles
                                         where playerProfiles.Username.Equals(username)
                                         select playerProfiles).First();
                    context.PlayerProfiles.Remove(playerProfile);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public Player GetPlayer(string username)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var players = (from gamer in context.Players
                                   where gamer.PlayerProfileUsername.Equals(username)
                                   select gamer).First();
                    var player = new Player
                    {
                        EmailAddress = players.EmailAddress,
                        Name = players.Name,
                        PaternalSurname = players.PaternalSurname,
                        MaternalSurname = players.MaternalSurname,
                    };
                    return player;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public PlayerProfile GetPlayerProfile(string emailAddress)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                          join player in context.Players
                                          on gamer.Username equals player.PlayerProfileUsername
                                          where player.EmailAddress.Equals(emailAddress)
                                          select gamer).First();
                    var playerProfile = new PlayerProfile
                    {
                        Username = playerProfiles.Username,
                        Password = playerProfiles.Password,
                    };
                    return playerProfile;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public PlayerProfile Login(string username, string password)
        {
            PlayerProfile playerProfile = null;
            using (ParlisContext context = new ParlisContext())
            {
                var playerProfiles = (from gamer in context.PlayerProfiles
                                      where gamer.Username.Equals(username) && gamer.Password.Equals(password)
                                      select gamer).FirstOrDefault();
                if (playerProfiles != null)
                {
                    playerProfile = new PlayerProfile
                    {
                        Username = playerProfiles.Username,
                        Password = playerProfiles.Password,
                        IsVerified = (bool)playerProfiles.IsVerified,
                    };
                }
            }
            return playerProfile;
        }

        public bool RegisterPlayer(Player player)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var players = new DataAccess.Player
                    {
                        EmailAddress = player.EmailAddress,
                        Name = player.Name,
                        PaternalSurname = player.PaternalSurname,
                        MaternalSurname = player.MaternalSurname,
                        PlayerProfileUsername = player.PlayerProfileUsername,
                    };

                    context.Players.Add(players);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }
        }

        public bool RegisterPlayerProfile(PlayerProfile playerProfile)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfiles = new DataAccess.PlayerProfile
                    {
                        Username = playerProfile.Username,
                        Password = playerProfile.Password,
                        IsVerified = playerProfile.IsVerified,
                    };

                    context.PlayerProfiles.Add(playerProfiles);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }
        }

        public bool SendMail(string username, string title, string message, int code)
        {
            try
            {
                string smtpServer = ConfigurationManager.AppSettings["SMTP_SERVER"];
                int port = int.Parse(ConfigurationManager.AppSettings["PORT"]);
                string emailAddress = ConfigurationManager.AppSettings["EMAIL_ADDRESS"];
                string password = ConfigurationManager.AppSettings["PASSWORD"];
                string addressee = GetPlayer(username).EmailAddress;

                var mailMessage = new MailMessage(emailAddress, addressee, title, (message + " " + code + "."))
                {
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailAddress, password),
                    EnableSsl = true,
                };
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }


        public bool UpdatePlayer(Player player)
        {
            try
            {
                var emailAddress = player.EmailAddress;
                using (ParlisContext context = new ParlisContext())
                {
                    var players = (from gamer in context.Players
                                   where gamer.EmailAddress.Equals(emailAddress)
                                   select gamer).First();
                    players.Name = player.Name;
                    players.PaternalSurname = player.PaternalSurname;
                    players.MaternalSurname = player.MaternalSurname;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public bool UpdatePlayerProfile(PlayerProfile playerProfile)
        {
            try
            {
                var username = playerProfile.Username;
                using (ParlisContext context = new ParlisContext())
                {
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                          where gamer.Username.Equals(username)
                                          select gamer).First();
                    playerProfiles.Password = playerProfile.Password;
                    playerProfiles.IsVerified = playerProfile.IsVerified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class Service : IMatchManagement, IChatManagement, IGameManagement
    {
        private static readonly List<int> matches = new List<int>();
        private static readonly Dictionary<string, int> playerProfilesByMatch = new Dictionary<string, int>();
        private static readonly Dictionary<string, int> playerProfilesByBoard = new Dictionary<string, int>();

        private static readonly List<Coin> coins = new List<Coin>();

        private static readonly Dictionary<int, List<Message>> messagesByMatch = new Dictionary<int, List<Message>>();
        private static readonly Dictionary<string, IMatchManagementCallback> playerProfiles = new Dictionary<string, IMatchManagementCallback>();
        private static readonly Dictionary<string, IChatManagementCallback> chats = new Dictionary<string, IChatManagementCallback>();
        private static readonly Dictionary<string, IGameManagementCallback> boards = new Dictionary<string, IGameManagementCallback>();

        //Board COmplement
        private Random randomResult;


        public bool CheckMatchExistence(int code)
        {
            return matches.Contains(code);
        }

        public void ConnectToBoard(string username, int code)
        {
            boards.Add(username, OperationContext.Current.GetCallbackChannel<IGameManagementCallback>());
            playerProfilesByBoard.Add(username, code);
            if (playerProfilesByBoard.Count == 4)
            {
                SetTurns();
                SetPlayerToPlay();
            }
        }

        public void ConnectToChat(string username, int code)
        {
            chats.Add(username, OperationContext.Current.GetCallbackChannel<IChatManagementCallback>());
            SetMessages(code);
        }

        public void ConnectToMatch(string username, int code)
        {
            playerProfilesByMatch.Add(username, code);
            playerProfiles.Add(username, OperationContext.Current.GetCallbackChannel<IMatchManagementCallback>());
            SetPlayerProfiles(code);
        }

        public void CreateMatch(int code)
        {
            matches.Add(code);
        }

        public void DisconnectFromBoard(string disconectedPlayerUsername)
        {
            boards.Remove(disconectedPlayerUsername);
            playerProfilesByBoard.Remove(disconectedPlayerUsername);
            chats.Remove(disconectedPlayerUsername);
            playerProfilesByMatch.Remove(disconectedPlayerUsername);
            playerProfiles.Remove(disconectedPlayerUsername);
            Console.WriteLine("Boards: "+boards.Count);
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    Console.WriteLine("Foreach-Boards Key: " + username+ " value:"+ playerProfile.Value);
                }
            }
            Console.WriteLine("chats: " + chats.Count);
            foreach (var playerProfile in chats)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    Console.WriteLine("Foreach-chats Key: " + username + " value:" + playerProfile.Value);
                }
            }
            Console.WriteLine("playerProfilesByBoard: " + playerProfilesByBoard.Count);
            foreach (var playerProfile in playerProfilesByBoard)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    Console.WriteLine("Foreach-playerProfilesByBoard Key: " + username + " value:" + playerProfile.Value);
                }
            }
            Console.WriteLine("playerProfilesByMatch: " + playerProfilesByMatch.Count);
            foreach (var playerProfile in playerProfilesByMatch)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    Console.WriteLine("Foreach-playerProfilesByMatch Key: " + username + " value:" + playerProfile.Value);
                }
            }
            Console.WriteLine("playerProfiles: " + playerProfiles.Count);
            foreach (var playerProfile in playerProfiles)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    Console.WriteLine("Foreach-playerProfiles Key: " + username + " value:" + playerProfile.Value);
                }
            }

            LeaveMatch(disconectedPlayerUsername);
        }

        public void DisconnectFromChat(string username)
        {
            chats.Remove(username);
        }

        public void DisconnectFromMatch(string username, int code)
        {
            playerProfilesByBoard.Remove(username);
            playerProfilesByMatch.Remove(username);
            playerProfiles.Remove(username);
            SetPlayerProfiles(code);
        }

        public List<Message> GetMessages(int code)
        {
            if (!messagesByMatch.ContainsKey(code))
            {
                List<Message> messages = new List<Message>();
                messagesByMatch.Add(code, messages);
            }
            return messagesByMatch[code];
        }

        void IMatchManagement.GetPlayerProfiles(string username, int code)
        {
            if (playerProfiles.ContainsKey(username))
            {
                OperationContext.Current.GetCallbackChannel<IMatchManagementCallback>().ReceivePlayerProfiles(new List<string>(playerProfiles.Keys));
            }
            else
            {
                OperationContext.Current.GetCallbackChannel<IMatchManagementCallback>().ReceivePlayerProfiles(GetPlayerProfiles(code));
            }
        }

        public List<string> GetPlayerProfiles(int code)
        {
            return playerProfilesByMatch.Where(playerProfile => playerProfile.Value == code)
                .Select(playerProfile => playerProfile.Key)
                .ToList();
        }

        public void SendMessage(int code, Message message)
        {
            messagesByMatch[code].Add(message);
            SetMessages(code);
        }

        public void SetPlayerProfiles(int code)
        {
            foreach (var playerProfile in playerProfilesByMatch)
            {
                if (playerProfile.Value.Equals(code))
                {
                    string username = playerProfile.Key;
                    playerProfiles[username].ReceivePlayerProfiles(GetPlayerProfiles(code));
                }
            }
        }

        public void SetMessages(int code)
        {
            foreach (var playerProfile in playerProfilesByMatch)
            {
                string username = playerProfile.Key;
                if (chats.ContainsKey(username))
                {
                    chats[username].ReceiveMessages(GetMessages(code));
                }
            }
        }

        public void SetBoardMatch()
        {
            foreach (var playerProfile in playerProfiles)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    playerProfiles[username].StarMatch();
                }
            }
        }

        void IGameManagement.GetCoinsForBoard(string username, int code)
        {
            if (playerProfiles.ContainsKey(username))
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceiveCoinsForBoard(new List<Coin>(coins));
            }
            else
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceiveCoinsForBoard(GetCoinsForBoard(code));
            }
        }
        public List<Coin> GetCoinsForBoard(int code)
        {
            List<Coin> CoinsTurn = new List<Coin>();
            if ((playerProfilesByBoard.Where(playerProfile => playerProfile.Value == code)) != null)
            {
                CoinsTurn = coins;
            }
            else
                CoinsTurn = null;
            return CoinsTurn;
        }

        public void SetCoinsForBoard(int code)
        {
            foreach (var playerProfile in playerProfilesByBoard)
            {
                if (playerProfile.Value.Equals(code))
                {
                    string username = playerProfile.Key;

                    boards[username].ReceiveCoinsForBoard(GetCoinsForBoard(code));
                }
            }

        }

        public void SetPlayerToPlay()
        {
            lock (playerProfilesByBoard)
            {
                foreach (var playerProfile in playerProfilesByBoard)
                {
                    SetCoinsForBoard(playerProfile.Value);

                }
            }
        }

        public void ThrowDice()
        {
            int diceResult;
            randomResult = new Random();
            diceResult = randomResult.Next(1, 7);
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowDiceResult(diceResult);
                }
            }
        }

        public void SetTurns()
        {
            randomResult = new Random();
            int randomPlayer, randomColorTeamValue;
            List<int> ColorTeamValues = new List<int>();
            ColorTeamValues.Add(0);
            ColorTeamValues.Add(1);
            ColorTeamValues.Add(2);
            ColorTeamValues.Add(3);
            List<string> players = new List<string>();
            players = playerProfilesByBoard.Keys.ToList();

            for (int i = 0; i < playerProfilesByBoard.Count; i++)

            {

                randomPlayer = randomResult.Next(players.Count);
                randomColorTeamValue = randomResult.Next(ColorTeamValues.Count);
                //PRUEBA Orden Rojo,Azul,Verde,Amarillo
                Coin coin = new Coin(ColorTeamValues[i]);

                //NORMAL Random
                //Coin coin = new Coin(ColorTeamValues[randomColorTeamValue]);
                coin.PlayerProfileUsername = players.ElementAt(randomPlayer);
                //ColorTeamValues.RemoveAt(randomColorTeamValue);
                players.RemoveAt(randomPlayer);
                coins.Add(coin);
            }
        }

        public void SetNextTurn()
        {
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowNextTurn();
                }
            }
        }

        public void SetCoinToMove(int turnPlayer)
        {
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].MoveInNormalPath(turnPlayer);
                }
            }
        }

        public void LeaveMatch(string disconectedPlayerUsername)
        {
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowDisconectedPlayer(disconectedPlayerUsername);
                }
            }

        }

        public bool RegisterMatchResult(PlayerProfile playerProfile)
        {
            DateTime today = DateTime.Now;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {

                    var match = new DataAccess.Match
                    {
                        Date = today,
                        PlayerProfileUsername = playerProfile.Username,
                    };
                    context.Matches.Add(match);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }
        }
    }
}