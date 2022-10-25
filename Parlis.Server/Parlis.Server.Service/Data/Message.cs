using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public int IdMessage { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public PlayerProfile Sender { get; set; }

        [DataMember]
        public PlayerProfile Receiver { get; set; }
    }
}
