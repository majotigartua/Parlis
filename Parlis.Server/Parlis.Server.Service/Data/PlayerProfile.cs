using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class PlayerProfile
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsVerified { get; set; }

        public PlayerProfile()
        {
        }
    }
}