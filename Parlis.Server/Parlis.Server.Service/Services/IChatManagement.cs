using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IChatManagementCallback))]
    public interface IChatManagement
    {
        [OperationContract(IsOneWay = true)]
        void CreateChat(int code);

        [OperationContract]
        void SendMessage(int code, Message message);
    }

    [ServiceContract]
    public interface IChatManagementCallback
    {
        [OperationContract]
        void ReceiveMessages(List<Message> messages);
    }
}