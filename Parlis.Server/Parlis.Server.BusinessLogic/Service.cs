using Parlis.Server.DataAccess;
using Parlis.Server.Service.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using Player = Parlis.Server.Service.Data.Player;
using PlayerProfile = Parlis.Server.Service.Data.PlayerProfile;

namespace Parlis.Server.BusinessLogic
{ 
    public partial class Service : IPlayerProfileManagement
    {
        public bool CheckPlayerExistence(Player player)
        {
            string emailAddress = player.EmailAddress;
            using (ParlisContext context = new ParlisContext())
            {
                int numberOfPlayers = (from players in context.Players
                                     where players.EmailAddress.Equals(emailAddress)
                                     select players).Count();
                return numberOfPlayers > 0;
            }
        }

        public bool CheckPlayerProfileExistence(PlayerProfile playerProfile)
        {
            string username = playerProfile.Username;
            using (ParlisContext context = new ParlisContext())
            {
                int numberOfPlayerProfiles = (from playerProfiles in context.PlayerProfiles
                                            where playerProfiles.Username.Equals(username)
                                            select playerProfiles).Count();
                return numberOfPlayerProfiles > 0;
            }
        }

        public bool DeletePlayer(Player player)
        {
            string emailAddress = player.EmailAddress;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var players = (from gamer in context.Players
                                                 where gamer.EmailAddress.Equals(emailAddress)
                                                 select gamer).First();
                    context.Players.Remove(players);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public bool DeletePlayerProfile(PlayerProfile playerProfile)
        {
            string username = playerProfile.Username;
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                                               where gamer.Username.Equals(username)
                                                               select gamer).First();
                    context.PlayerProfiles.Remove(playerProfiles);
                    context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
        }

        public Player GetPlayer(PlayerProfile playerProfile)
        {
            string username = playerProfile.Username;
            using (ParlisContext context = new ParlisContext())
            {
                var players = (from gamer in context.Players
                                            where gamer.PlayerProfileUsername.Equals(username)
                                            select gamer).First();
                var player = new Player()
                {
                    EmailAddress = players.EmailAddress,
                    Name = players.Name,
                    PaternalSurname = players.PaternalSurname,
                    MaternalSurname = players.MaternalSurname,
                };
                return player;
            }
        }

        public PlayerProfile Login(string username, string password)
        {
            using (ParlisContext context = new ParlisContext())
            {
                var playerProfiles = (from gamer in context.PlayerProfiles
                                      where gamer.Username.Equals(username) && gamer.Password.Equals(password)
                                      select gamer).FirstOrDefault();
                if (playerProfiles != null)
                {
                    var playerProfile = new PlayerProfile()
                    {
                        Username = playerProfiles.Username,
                        Password = playerProfiles.Password,
                        IsVerified = (bool) playerProfiles.IsVerified,
                    };
                    return playerProfile;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool RegisterPlayer(Player player)
        {
            using (ParlisContext context = new ParlisContext())
            {
                var players = new DataAccess.Player()
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
                var playerProfiles = new DataAccess.PlayerProfile()
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
        public bool SendMail(PlayerProfile playerProfile, string title, string message, int code)
        {
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            string emailAddress = ConfigurationManager.AppSettings["EmailAddress"];
            string password = ConfigurationManager.AppSettings["Password"];
            using (ParlisContext context = new ParlisContext())
            {
                string addressee = (from player in context.Players
                                       where player.PlayerProfileUsername.Equals(playerProfile.Username)
                                       select player).First().EmailAddress;
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
        }

        public bool UpdatePlayer(Player player)
        {
            var emailAddress = player.EmailAddress;
            using (ParlisContext context = new ParlisContext())
            {
                try
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
                try
                {
                    var playerProfiles = (from gamer in context.PlayerProfiles
                                                    where gamer.Username.Equals(username)
                                                    select gamer).First();
                    playerProfiles.Password = playerProfile.Password;
                    playerProfiles.IsVerified = playerProfile.IsVerified;
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

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class Service : IMatchManagement
    {
        public static Dictionary<PlayerProfile, IMatchManagementCallback> players = new Dictionary<PlayerProfile, IMatchManagementCallback>();

        public void Connect(PlayerProfile playerProfile)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IMatchManagementCallback>();
            players.Add(playerProfile, connection);
        }
    }
}