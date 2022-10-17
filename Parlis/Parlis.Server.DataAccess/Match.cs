namespace Parlis.Server.DataAccess
{    
    public partial class Match
    {
        public int IdMatch { get; set; }
        public System.DateTime Date { get; set; }
        public string PlayerProfileUsername { get; set; }
    
        public virtual PlayerProfile PlayerProfile { get; set; }
    }
}
