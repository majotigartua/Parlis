using System;
using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Player : IEquatable<Player>
    {
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PaternalSurname { get; set; }
        [DataMember]
        public string MaternalSurname { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Player()
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player other)
        {
            return ((other != null) &&
               (EmailAddress == other.EmailAddress) &&
               (Name == other.Name) &&
               (PaternalSurname == other.PaternalSurname) &&
               (MaternalSurname == other.MaternalSurname))
               ? true : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EmailAddress, Name, PaternalSurname, MaternalSurname, PlayerProfileUsername);
        }
    }
}