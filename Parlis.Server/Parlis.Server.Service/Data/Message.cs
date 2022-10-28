using System.Runtime.Serialization;
using System.ServiceModel;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        [OperationContract]
        override
        public string ToString()
        {
            return $"{PlayerProfileUsername} : {Body}";
        }

        public Message()
        {
        }
    }
}