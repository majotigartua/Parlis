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
        private static readonly Dictionary<string, int> turns = new Dictionary<string, int>();

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

        public void DisconnectFromChat(string username)
        {
            chats.Remove(username);
        }

        public void DisconnectFromMatch(string username, int code)
        {
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


        //Board methods 


        #region Board and gameplay methods
        
        //Conexion to server and boards
        public void ConnectToBoard(string username, int code)
        {
            playerProfilesByBoard.Add(username, code);
            boards.Add(username, OperationContext.Current.GetCallbackChannel<IGameManagementCallback>());
            if (playerProfilesByBoard.Count == 4)
            {
                SetTurns();
                SetPlayerToPlay();

            }
        }
        public void DisconnectFromBoard(string username)
        {
            boards.Remove(username);
        }
        void IGameManagement.GetPlayerProfilesForBoard(string username, int code)
        {
            if (playerProfiles.ContainsKey(username))
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceivePlayerProfilesForBoard(new Dictionary<string, int>(turns));
            }
            else
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceivePlayerProfilesForBoard(GetPlayerProfilesForBoard(code));
            }
        }
        public Dictionary<string, int> GetPlayerProfilesForBoard(int code)
        {
            Dictionary<string, int> playerProfilesTurns = new Dictionary<string, int>();
            if ((playerProfilesByBoard.Where(playerProfile => playerProfile.Value == code)) != null)
            {
                playerProfilesTurns = turns;
            }
            else
                playerProfilesTurns = null;
            return playerProfilesTurns;
        }
        public void SetPlayerProfilesForBoard(int code)
        {
                foreach (var playerProfile in playerProfilesByBoard)
                {
                    if (playerProfile.Value.Equals(code))
                    {
                        string username = playerProfile.Key;

                        boards[username].ReceivePlayerProfilesForBoard(GetPlayerProfilesForBoard(code));
                    }
                }
            
        }
        public void SetPlayerToPlay()
        {
            lock (playerProfilesByBoard)
            {
                foreach (var playerProfile in playerProfilesByBoard)
                {
                    SetPlayerProfilesForBoard(playerProfile.Value);
                    
                }
            }
        }

        //Dice Methods
        public void SetDiceResult()
        {
            randomResult = new Random();
            int diceResult = randomResult.Next(1, 7);
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowDiceResult(diceResult);
                }
            }
        }
        //Turns Methods

        public void SetTurns()
        {
            randomResult = new Random();
            int randomPlayer, randomColorTeam;
            List<int> colorTeam = new List<int>();
            colorTeam.Add(1);//red
            colorTeam.Add(2);//blue
            colorTeam.Add(3);//green
            colorTeam.Add(4);//yellow
            for (int i = 0; i < playerProfilesByBoard.Count; i++)
            
            {
                do
                {
                    randomPlayer = randomResult.Next(playerProfilesByBoard.Count);
                } while (turns.ContainsKey(playerProfilesByBoard.ElementAt(randomPlayer).Key));
                do
                {
                    randomColorTeam = randomResult.Next(0,4);
                } while (turns.ContainsValue(colorTeam[randomColorTeam]));
                turns.Add(playerProfilesByBoard.ElementAt(randomPlayer).Key, colorTeam[randomColorTeam]);
            }
        }

        public void StartGame()
        {
            bool winnerPlayer = false;
            int turn = 0;
            string player;
            do
            {
                player = turns.ElementAt(turn).Key;
                boards[player].ShowNextTurn(turn);

                SetDiceResult();
                if (turn > 2) {
                    turn = -1;
                }
                turn++;
            }while(!winnerPlayer);
            StartGame();
        
        
        }


        public void SetNextTurn(int turn)
        {
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowNextTurn(turn);
                }
            }
        }
        #endregion 

    }
}