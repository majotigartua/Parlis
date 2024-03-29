﻿using Match = Parlis.Server.Service.Data.Match;
using Parlis.Server.Service.Data;
using Parlis.Server.DataAccess;
using Parlis.Server.Service.Services;
using Player = Parlis.Server.Service.Data.Player;
using PlayerProfile = Parlis.Server.Service.Data.PlayerProfile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;

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
            bool isDeleted;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var player = (from players in context.Players
                                  where players.EmailAddress.Equals(emailAddress)
                                  select players).First();
                    context.Players.Remove(player);
                    context.SaveChanges();
                    isDeleted = true;
                }
                catch (Exception)
                {
                    isDeleted = false;
                }
            }
            return isDeleted;
        }

        public bool DeletePlayerProfile(string username)
        {
            bool isDeleted;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfile = (from playerProfiles in context.PlayerProfiles
                                         where playerProfiles.Username.Equals(username)
                                         select playerProfiles).First();
                    context.PlayerProfiles.Remove(playerProfile);
                    context.SaveChanges();
                    isDeleted = true;
                }
                catch (Exception)
                {
                    isDeleted = false;
                }
            }
            return isDeleted;
        }

        public Player GetPlayer(string username)
        {
            Player player;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var players = (from gamer in context.Players
                                   where gamer.PlayerProfileUsername.Equals(username)
                                   select gamer).First();
                    player = new Player
                    {
                        EmailAddress = players.EmailAddress,
                        Name = players.Name,
                        PaternalSurname = players.PaternalSurname,
                        MaternalSurname = players.MaternalSurname,
                    };
                }
                catch (Exception)
                {
                    player = null;
                }
            }
            return player;
        }

        public PlayerProfile GetPlayerProfile(string emailAddress)
        {
            PlayerProfile playerProfile;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                          join player in context.Players
                                          on gamer.Username equals player.PlayerProfileUsername
                                          where player.EmailAddress.Equals(emailAddress)
                                          select gamer).First();
                    playerProfile = new PlayerProfile
                    {
                        Username = playerProfiles.Username,
                        Password = playerProfiles.Password,
                    };
                }
                catch (Exception)
                {
                    return playerProfile = null;
                }
            }
            return playerProfile;
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
            bool isRegistered;
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
                    isRegistered = true;
                }
                catch (Exception)
                {
                    isRegistered = false;
                }
            }
            return isRegistered;
        }

        public bool RegisterPlayerProfile(PlayerProfile playerProfile)
        {
            bool isRegistered;
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
                    isRegistered = true;
                }
                catch (Exception)
                {
                    isRegistered = false;
                }
            }
            return isRegistered;
        }

        public bool SendMail(string username, string title, string message, int code)
        {
            bool isSent;
            string smtpServer = ConfigurationManager.AppSettings["SMTP_SERVER"];
            int port = int.Parse(ConfigurationManager.AppSettings["PORT"]);
            string emailAddress = ConfigurationManager.AppSettings["EMAIL_ADDRESS"];
            string password = ConfigurationManager.AppSettings["PASSWORD"];
            string addressee = GetPlayer(username).EmailAddress;
            try
            {
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
                isSent = true;
            }
            catch (Exception)
            {
                isSent = false;
            }
            return isSent;
        }


        public bool UpdatePlayer(Player player)
        {
            bool isUpdated;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var emailAddress = player.EmailAddress;
                    var players = (from gamer in context.Players
                                   where gamer.EmailAddress.Equals(emailAddress)
                                   select gamer).First();
                    players.Name = player.Name;
                    players.PaternalSurname = player.PaternalSurname;
                    players.MaternalSurname = player.MaternalSurname;
                    context.SaveChanges();
                    isUpdated = true;
                }
                catch (Exception)
                {
                    isUpdated = false;
                }
            }
            return isUpdated;
        }

        public bool UpdatePlayerProfile(PlayerProfile playerProfile)
        {
            bool isUpdated;
            using (ParlisContext context = new ParlisContext())
            { 
                try
                {
                    var username = playerProfile.Username;
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                          where gamer.Username.Equals(username)
                                          select gamer).First();
                    playerProfiles.Password = playerProfile.Password;
                    playerProfiles.IsVerified = playerProfile.IsVerified;
                    context.SaveChanges();
                    isUpdated = true;
                }
                catch (Exception)
                {
                    isUpdated = false;
                }
            }
            return isUpdated;
        }
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class Service : IChatManagement, IGameManagement, IMatchManagement
    {
        private static readonly List<Coin> coins = new List<Coin>();
        private static readonly List<int> matches = new List<int>();
        private static readonly Dictionary<string, int> playerProfilesByMatch = new Dictionary<string, int>();
        private static readonly Dictionary<int, List<Message>> messagesByMatch = new Dictionary<int, List<Message>>();
        private static readonly Dictionary<string, int> playerProfilesByBoard = new Dictionary<string, int>();
        private static readonly Dictionary<string, IMatchManagementCallback> playerProfiles = new Dictionary<string, IMatchManagementCallback>();
        private static readonly Dictionary<string, IChatManagementCallback> chats = new Dictionary<string, IChatManagementCallback>();
        private static readonly Dictionary<string, IGameManagementCallback> boards = new Dictionary<string, IGameManagementCallback>();

        public void ConnectToChat(string username, int code)
        {
            chats.Add(username, OperationContext.Current.GetCallbackChannel<IChatManagementCallback>());
            SetMessages(code);
        }

        public void DisconnectFromChat(string username)
        {
            chats.Remove(username);
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

        public void SendMessage(int code, Message message)
        {
            messagesByMatch[code].Add(message);
            SetMessages(code);
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

        public void ConnectToBoard(string username, int code)
        {
            boards.Add(username, OperationContext.Current.GetCallbackChannel<IGameManagementCallback>());
            playerProfilesByBoard.Add(username, code);
            if (playerProfilesByBoard.Count.Equals(Constants.NUMBER_OF_PLAYER_PROFILES_PER_MATCH))
            {
                SetTurns();
                SetPlayerToPlay();
            }
        }

        public void DisconnectFromBoard(string username)
        {
            playerProfilesByBoard.Remove(username);
            playerProfilesByMatch.Remove(username);
            boards.Remove(username);
            chats.Remove(username);
            playerProfiles.Remove(username);
            LeaveMatch(username);
        }

        void IGameManagement.GetCoinsByBoard(string username, int code)
        {
            if (playerProfiles.ContainsKey(username))
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceiveCoinsForBoard(new List<Coin>(coins));
            }
            else
            {
                OperationContext.Current.GetCallbackChannel<IGameManagementCallback>().ReceiveCoinsForBoard(GetCoinsByBoard(code));
            }
        }

        public List<Coin> GetCoinsByBoard(int code)
        {
            return (playerProfilesByBoard.Where(playerProfile => playerProfile.Value == code) != null) ? coins : null;
        }

        public void LeaveMatch(string username)
        {
            foreach (var playerProfile in boards)
            {
                string playerProfileUsername = playerProfile.Key;
                if (playerProfiles.ContainsKey(playerProfileUsername))
                {
                    boards[playerProfileUsername].ShowDisconnectedPlayerProfile(username);
                }
            }
        }

        public void SetBoards()
        {
            foreach (var playerProfile in playerProfiles)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    playerProfiles[username].StartMatch();
                }
            }
        }

        public void SetCoinsByBoard(int code)
        {
            foreach (var playerProfile in playerProfilesByBoard)
            {
                if (playerProfile.Value.Equals(code))
                {
                    string username = playerProfile.Key;
                    boards[username].ReceiveCoinsForBoard(GetCoinsByBoard(code));
                }
            }
        }

        public void SetCoinToMove(int turn)
        {
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].MoveInNormalPath(turn);
                }
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

        public bool RegisterMatch(Match match)
        {
            bool isRegistered;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var matches = new DataAccess.Match
                    {
                        Date = match.Date,
                        PlayerProfileUsername = match.PlayerProfileUsername,
                    };
                    context.Matches.Add(matches);
                    context.SaveChanges();
                    isRegistered = true;
                }
                catch (Exception)
                {
                    isRegistered = false;
                }
            }
            return isRegistered;
        }

        public void SetPlayerToPlay()
        {
            lock (playerProfilesByBoard)
            {
                foreach (var playerProfile in playerProfilesByBoard)
                {
                    SetCoinsByBoard(playerProfile.Value);
                }
            }
        }

        public void SetTurns()
        {
            List<int> colorTeamValues = new List<int> {
                0,
                1,
                2,
                3
            };
            List<string> playerProfiles = playerProfilesByBoard.Keys.ToList();
            for (int turn = Constants.NUMBER_OF_PLAYER_PROFILES_PER_EMPTY_MATCH; turn < playerProfilesByBoard.Count; turn++)
            {
                int randomColorTeamValue = Constants.Random.Next(colorTeamValues.Count);
                int randomPlayer = Constants.Random.Next(playerProfiles.Count);
                Coin coin = new Coin(colorTeamValues[randomColorTeamValue])
                {
                    PlayerProfileUsername = playerProfiles.ElementAt(randomPlayer)
                };
                colorTeamValues.RemoveAt(randomColorTeamValue);
                playerProfiles.RemoveAt(randomPlayer);
                coins.Add(coin);
            }
        }

        public void ThrowDice()
        {
            int diceResult = Constants.Random.Next(1, 7);
            foreach (var playerProfile in boards)
            {
                string username = playerProfile.Key;
                if (playerProfiles.ContainsKey(username))
                {
                    boards[username].ShowDiceResult(diceResult);
                }
            }
        }

        public bool CheckMatchExistence(int code)
        {
            return matches.Contains(code);
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

        public void DisconnectFromMatch(string username, int code)
        {
            playerProfilesByBoard.Remove(username);
            playerProfilesByMatch.Remove(username);
            playerProfiles.Remove(username);
            SetPlayerProfiles(code);
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

        public void ExpelPlayerProfile(string username)
        {
            if (playerProfiles.ContainsKey(username))
            {
                playerProfiles[username].ExpelPlayerProfileFromMatch(username);
                int code = playerProfilesByMatch[username];
                DisconnectFromMatch(username, code);
            }
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
    }
}