using System.Runtime.Serialization;
using System.ServiceModel;

namespace Parlis.Server.Service.Data
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Content { get; set; }

        public Message()
        {
        }

        [OperationContract]
        override
        public string ToString()
        {
            return $"{Username}: {Content}";
        }
    }
}