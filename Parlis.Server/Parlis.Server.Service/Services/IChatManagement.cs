using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IChatManagementCallback))]
    public interface IChatManagement
    {
        [OperationContract(IsOneWay = true)]
        void ConnectToChat(int code);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message, int code);
    }

    [ServiceContract]
    public interface IChatManagementCallback
    {
        [OperationContract]
        void ReceiveMessages(List<Message> messages);
    }
}