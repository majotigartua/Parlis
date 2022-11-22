using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IGameManagementCallback))]
    public interface IGameManagement
    {
        [OperationContract(IsOneWay = true)]
        void GetCoinsForBoard(string username, int code);
        [OperationContract(IsOneWay = true)]
        void ThrowDice();
        [OperationContract(IsOneWay = true)]
        void SetNextTurn(int colorTeamValue);
        [OperationContract(IsOneWay = true)]
        void SetCoinToMove(int turnPlayer);


        //Borrador
        [OperationContract(IsOneWay = true)]
        void ConnectToBoard(string username, int code);

        [OperationContract]
        void DisconnectFromBoard(string username);
    }

    [ServiceContract]
    public interface IGameManagementCallback
    {
        [OperationContract]
        void ReceiveCoinsForBoard(List<Coin> coins);
        [OperationContract]
        void ShowDiceResult(int diceResult);
        [OperationContract]
        void ShowNextTurn(int colorTeamValue);
        [OperationContract]
        void ShowCoinMoved(int turnPlayer);
    }
}