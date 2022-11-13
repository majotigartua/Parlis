using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IGameManagementCallback))]
    public interface IGameManagement
    {
        [OperationContract(IsOneWay = true)]
        void ConnectToBoard(string username, int code);

        [OperationContract]
        void DisconnectFromBoard(string username);

        [OperationContract(IsOneWay = true)]
        void SendMove(int result, Coin coin);

        [OperationContract(IsOneWay = true)]
        void GetPlayerProfilesForBoard(string username, int code);

        [OperationContract(IsOneWay = true)]
        void SetDiceResult();

        [OperationContract(IsOneWay = true)]
        void StartGame();
    }

    [ServiceContract]
    public interface IGameManagementCallback
    {
        [OperationContract]
        void ReceiveMove(Coin coin);
        [OperationContract]
        void ReceivePlayerProfilesForBoard(Dictionary<string, int> playerProfilesTurns);

        [OperationContract]
        void ShowDiceResult(int result);

        [OperationContract]
        void ShowNextTurn(int turn);
    }
}