using Parlis.Server.DataAccess;
using Parlis.Server.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class Service : IMatchManagement
    {
        public static Dictionary<IMatchManagementCallback, Player> players = new Dictionary<IMatchManagementCallback, Player>();

        public void Connect(PlayerProfile playerProfile)
        {
            using (ParlisContext context = new ParlisContext())
            {
                Player player = (from players in context.Players
                                 where players.PlayerProfileUsername.Equals(playerProfile.Username)
                                 select players).First();
                var connection = OperationContext.Current.GetCallbackChannel<IMatchManagementCallback>();
                players.Add(connection, player);
            }
        }
    }
}