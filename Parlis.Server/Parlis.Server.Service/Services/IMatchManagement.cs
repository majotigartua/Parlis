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
        void GetPlayerProfiles(string username, int code);

        [OperationContract(IsOneWay = true)]
        void SetBoards();
        [OperationContract(IsOneWay = true)]
        void SelectPlayerToExpel(string ExpeledPlayerUSername);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
        [OperationContract]
        void ReceivePlayerProfiles(List<string> playerProfiles);
        [OperationContract]
        void ExpelPlayerFromMatch(string ExpeledPlayerUSername);

        [OperationContract]
        void StartMatch();
    }
}