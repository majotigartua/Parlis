namespace Parlis.Server.DataAccess
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class PlayerProfile
    {
        public PlayerProfile()
        {
            this.Matches = new ObservableCollection<Match>();
            this.Players = new ObservableCollection<Player>();
        }

        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public virtual ObservableCollection<Match> Matches { get; set; }
        [DataMember]
        public virtual ObservableCollection<Player> Players { get; set; }
    }
}