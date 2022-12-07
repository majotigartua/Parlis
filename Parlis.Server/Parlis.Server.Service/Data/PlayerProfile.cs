using System;
using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class PlayerProfile : IEquatable<PlayerProfile>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerProfile);
        }

        public bool Equals(PlayerProfile player)
        {
            return player != null &&
               Username == player.Username &&
               Password == player.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Password, IsVerified);
        }

    }
}