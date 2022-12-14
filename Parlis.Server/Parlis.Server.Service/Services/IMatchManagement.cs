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
        void ConnectToMatch(string username, int code);

        [OperationContract]
        void CreateMatch(int code);

        [OperationContract]
        void DisconnectFromMatch(string username, int code);

        [OperationContract(IsOneWay = true)]
        void ExpelPlayerProfile(string username);

        [OperationContract(IsOneWay = true)]
        void GetPlayerProfiles(string username, int code);

        [OperationContract(IsOneWay = true)]
        void SetBoards();
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
        [OperationContract]
        void ExpelPlayerProfileFromMatch(string username);

        [OperationContract]
        void ReceivePlayerProfiles(List<string> playerProfiles);

        [OperationContract]
        void StartMatch();
    }
}