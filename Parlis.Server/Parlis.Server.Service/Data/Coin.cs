using System.Runtime.Serialization;
using System.ServiceModel;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Coin
    {
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public int ColorTeam { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Coin(int colorValue)
        {
            this.ColorTeam = colorValue; 
        }
    }
}
