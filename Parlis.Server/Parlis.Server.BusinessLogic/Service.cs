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
                var player = (from players in context.Players
                              where players.EmailAddress.Equals(emailAddress)
                              select players).First();
                try
                {
                    context.Players.Remove(player);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool DeletePlayerProfile(string username)
        {
            using (ParlisContext context = new ParlisContext())
            {
                var playerProfile = (from playerProfiles in context.PlayerProfiles
                                     where playerProfiles.Username.Equals(username)
                                     select playerProfiles).First();
                try
                {
                    context.PlayerProfiles.Remove(playerProfile);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public Player GetPlayer(string username)
        {
            using (ParlisContext context = new ParlisContext())
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
        }

        public PlayerProfile GetPlayerProfile(string emailAddress)
        {
            using (ParlisContext context = new ParlisContext())
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
                        IsVerified = (bool) playerProfiles.IsVerified,
                    };
                }
            }
            return playerProfile;
        }

        public bool RegisterPlayer(Player player)
        {
            using (ParlisContext context = new ParlisContext())
            {
                var players = new DataAccess.Player
                {
                    EmailAddress = player.EmailAddress,
                    Name = player.Name,
                    PaternalSurname = player.PaternalSurname,
                    MaternalSurname = player.MaternalSurname,
                    PlayerProfileUsername = player.PlayerProfileUsername,
                };
                try
                {
                    context.Players.Add(players);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool RegisterPlayerProfile(PlayerProfile playerProfile)
        {
            using (ParlisContext context = new ParlisContext())
            {
                var playerProfiles = new DataAccess.PlayerProfile
                {
                    Username = playerProfile.Username,
                    Password = playerProfile.Password,
                    IsVerified = playerProfile.IsVerified,
                };
                try
                {
                    context.PlayerProfiles.Add(playerProfiles);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool SendMail(string username, string title, string message, int code)
        {
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        public bool UpdatePlayer(Player player)
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
                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool UpdatePlayerProfile(PlayerProfile playerProfile)
        {
            var username = playerProfile.Username;
            using (ParlisContext context = new ParlisContext())
            {
                var playerProfiles = (from gamer in context.PlayerProfiles
                                      where gamer.Username.Equals(username)
                                      select gamer).First();
                playerProfiles.Password = playerProfile.Password;
                playerProfiles.IsVerified = playerProfile.IsVerified;
                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class Service : IMatchManagement, IChatManagement
    {
        private static readonly List<int> matches = new List<int>();
        private static readonly Dictionary<string, int> playerProfilesByMatch = new Dictionary<string, int>();
        private static readonly Dictionary<int, List<Message>> messagesByMatch = new Dictionary<int, List<Message>>();
        private static readonly Dictionary<string, IMatchManagementCallback> playerProfiles = new Dictionary<string, IMatchManagementCallback>();
        private static readonly Dictionary<string, IChatManagementCallback> chats = new Dictionary<string, IChatManagementCallback>();

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
    }
}