using Parlis.Server.DataAccess;
using System.Linq;

namespace Parlis.Server.BusinessLogic
{
    public class PlayerProfileManagement : IPlayerProfileManagement
    {
        private readonly Utilities _utilities = new Utilities();

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

            using (ParlisContext context = new ParlisContext())
            {
                int playerProfileCounter = (from playerProfiles in context.PlayerProfiles
                                            where playerProfiles.Username.Equals(username)
                                            select playerProfiles).Count();
                return playerProfileCounter > 0;
            }
        }

        public bool Login(string username, string password)
        {
            string hashedPassword = _utilities.ComputeSHA256Hash(password);
            using (ParlisContext context = new ParlisContext())
            {
                int playerProfileCounter = (from playerProfiles in context.PlayerProfiles
                                            where playerProfiles.Username == username && playerProfiles.Password == hashedPassword
                                            select playerProfiles).Count();
                return playerProfileCounter > 0;
            }
        }

        public bool RegisterPlayer(Player player)
        {
            using (ParlisContext context = new ParlisContext())
            {
                context.Players.Add(player);
                context.SaveChanges();
                return (CheckPlayerExistence(player));
            }
        }

        public bool RegisterPlayerProfile(PlayerProfile playerProfile)
        {
            using (ParlisContext context = new ParlisContext())
            {
                context.PlayerProfiles.Add(playerProfile);
                context.SaveChanges();
                return (CheckPlayerProfileExistence(playerProfile));
            }
        }
    }
}