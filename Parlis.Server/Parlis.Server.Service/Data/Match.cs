using System;
using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Match
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Match()
        {
        }   
    }
}