using Parlis.Server.DataAccess;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Parlis.Server.BusinessLogic
{
    public class PlayerProfileManagement : IPlayerProfileManagement
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
                                            where playerProfiles.Username == username && playerProfiles.Password == password
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
}