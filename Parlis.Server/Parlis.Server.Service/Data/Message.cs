using System.Runtime.Serialization;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Message()
        {
        }
    }
}