using System;

namespace Parlis.Server.DataAccess
{
    public partial class Match
    {
        public int IdMatch { get; set; }
        public DateTime Date { get; set; }
        public string PlayerProfileUsername { get; set; }
    
        public virtual PlayerProfile PlayerProfile { get; set; }
    }
}