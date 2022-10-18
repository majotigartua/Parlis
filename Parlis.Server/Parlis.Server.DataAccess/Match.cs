using System.Runtime.Serialization;

namespace Parlis.Server.DataAccess
{
    [DataContract]    
    public partial class Match
    {
        [DataMember]
        public int IdMatch { get; set; }
        [DataMember]
        public System.DateTime Date { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        [DataMember]
        public virtual PlayerProfile PlayerProfile { get; set; }
    }
}
