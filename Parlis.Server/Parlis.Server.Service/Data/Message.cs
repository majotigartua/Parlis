using System.Runtime.Serialization;
using System.ServiceModel;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public string PlayerProfileUsername { get; set; }

        public Message()
        {
        }

        [OperationContract]
        override
        public string ToString()
        {
            return $"{PlayerProfileUsername}: {Content}";
        }
    }
}