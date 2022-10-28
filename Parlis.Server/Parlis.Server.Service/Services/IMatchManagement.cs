using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IMatchManagementCallback))]
    public interface IMatchManagement
    {
        [OperationContract]
        bool CheckMatchExistence(int code);

        [OperationContract(IsOneWay = true)]
        void Connect(int code, string username);

        [OperationContract]
        void CreateMatch(int code);

        [OperationContract]
        void Disconnect(int code, string username);

        [OperationContract(IsOneWay = true)]
        void GetPlayerProfiles(int code);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
        [OperationContract]
        void ReceivePlayerProfiles(List<string> playerProfiles);
    }
}