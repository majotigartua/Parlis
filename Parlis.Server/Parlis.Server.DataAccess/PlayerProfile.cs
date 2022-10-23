namespace Parlis.Server.DataAccess
{
    using System.Collections.ObjectModel;
    
    public partial class PlayerProfile
    {
        public PlayerProfile()
        {
            Matches = new ObservableCollection<Match>();
            Players = new ObservableCollection<Player>();
        }
    
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? IsVerified { get; set; }
    
        public virtual ObservableCollection<Match> Matches { get; set; }
        public virtual ObservableCollection<Player> Players { get; set; }
    }
}