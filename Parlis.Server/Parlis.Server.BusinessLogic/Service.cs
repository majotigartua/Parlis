using Parlis.Server.DataAccess;
using Parlis.Server.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;

namespace Parlis.Server.BusinessLogic
{ 
    public partial class Service : IPlayerProfileManagement
    {
        public bool CheckPlayerExistence(Player player)
        {
            string emailAddress = player.EmailAddress;
            using (ParlisContext context = new ParlisContext())
            {
                int playerCounter = (from players in context.Players
                                     where players.EmailAddress.Equals(emailAddress)
                                     select players).Count();
                return playerCounter > 0;
            }
        }

        public bool CheckPlayerProfileExistence(PlayerProfile playerProfile)
        {
            string username = playerProfile.Username;
            string password = playerProfile.Password;
            using (ParlisContext context = new ParlisContext())
            {
                int playerProfileCounter = (from playerProfiles in context.PlayerProfiles
                                            where playerProfiles.Username.Equals(username)
                                            select playerProfiles).Count();
                return playerProfileCounter > 0;
            }
        }

        public Player GetPlayer(PlayerProfile playerProfile)
        {
            var username = playerProfile.Username;
            using (ParlisContext context = new ParlisContext())
            {
                Player player = (from players in context.Players
                                 where players.PlayerProfileUsername.Equals(username)
                                 select players).First();
                return player;
            }
        }

        public bool Login(PlayerProfile playerProfile)
        {
            string username = playerProfile.Username;
            string password = playerProfile.Password;
            using (ParlisContext context = new ParlisContext())
            {
                int playerProfileCounter = (from playerProfiles in context.PlayerProfiles
                                            where playerProfiles.Username.Equals(username) && playerProfiles.Password.Equals(password)
                                            select playerProfiles).Count();
                return playerProfileCounter > 0;
            }
        }

        public bool RegisterPlayer(Player player)
        {
            using (ParlisContext context = new ParlisContext())
            {
                try
                {
                    context.Players.Add(player);
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
                try
                {
                    context.PlayerProfiles.Add(playerProfile);
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
            int port = Int32.Parse(ConfigurationManager.AppSettings["Port"]);
            string emailAddress = ConfigurationManager.AppSettings["EmailAddress"];
            string password = ConfigurationManager.AppSettings["Password"];
            using (ParlisContext context = new ParlisContext())
            {
                string addressee = (from player in context.Players
                                       where player.PlayerProfileUsername.Equals(playerProfile.Username)
                                       select player).First().EmailAddress;
                try
                {
                    var smtpClient = new SmtpClient(smtpServer, port)
                    {
                        Credentials = new NetworkCredential(emailAddress, password),
                        EnableSsl = true,
                    };
                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress(emailAddress, "Parlis."),
                        Subject = title,
                        Body = message + code,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(addressee);
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
                    Player players = (from gamer in context.Players
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
                    PlayerProfile playerProfiles = (from gamer in context.PlayerProfiles
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