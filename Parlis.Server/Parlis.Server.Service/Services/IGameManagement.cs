using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IGameManagementCallback))]
    public interface IGameManagement
    {
        [OperationContract(IsOneWay = true)]
        void GetCoinsByBoard(string username, int code);

        [OperationContract(IsOneWay = true)]
        void ThrowDice();

        [OperationContract(IsOneWay = true)]
        void SetNextTurn();

        [OperationContract(IsOneWay = true)]
        void SetCoinToMove(int turn);

        [OperationContract(IsOneWay = true)]
        void LeaveMatch(string username);

        [OperationContract(IsOneWay = true)]
        void ConnectToBoard(string username, int code);

        [OperationContract]
        void DisconnectFromBoard(string username);

        [OperationContract]
        bool RegisterMatch(PlayerProfile playerProfile);
    }

    [ServiceContract]
    public interface IGameManagementCallback
    {
        [OperationContract]
        void ReceiveCoinsForBoard(List<Coin> coins);

        [OperationContract]
        void ShowDiceResult(int diceResult);

        [OperationContract]
        void ShowNextTurn();

        [OperationContract]
        void MoveInNormalPath(int turnPlayer);

        [OperationContract]
        void ShowDisconectedPlayer(string username);
    }
}