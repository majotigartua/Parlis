﻿using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Player
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
    }
}