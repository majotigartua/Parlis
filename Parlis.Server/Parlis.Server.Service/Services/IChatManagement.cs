using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IChatManagementCallback))]
    public interface IChatManagement
    {
        [OperationContract(IsOneWay = true)]
        void ConnectToChat(string username, int code);

        [OperationContract]
        void DisconnectFromChat(string username);

        [OperationContract(IsOneWay = true)]
        void SendMessage(int code, Message message);
    }

    [ServiceContract]
    public interface IChatManagementCallback
    {
        [OperationContract]
        void ReceiveMessages(List<Message> messages);
    }
}