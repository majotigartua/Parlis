using Parlis.Server.Service.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IMatchManagementCallback))]
    public interface IMatchManagement
    {
        [OperationContract]
        bool CheckMatchExistence(int code);

        [OperationContract]
        bool CheckPlayerProfile(string username);

        [OperationContract(IsOneWay = true)]
        void Connect(int code);

        [OperationContract]
        void Disconnect(string username);

        [OperationContract]
        void CreateMatch(int code);

        [OperationContract(IsOneWay = true)]
        void JoinMatch(string username, int code);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
        [OperationContract]
        void GetPlayerProfiles(List<string> playerProfiles);
    }
}