using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatManagement
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, int code);

    }

    [ServiceContract]
    public interface IChatCallback
    {
        [OperationContract]
        void ReceiveMessage(List<string> messages);
    }
}