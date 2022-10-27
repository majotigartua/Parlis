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
        void Connect(string username, int code);

        [OperationContract]
        void CreateMatch(int code);

        [OperationContract]
        void Disconnect(string username, int code);

        [OperationContract(IsOneWay = true)]
        void GetPlayerProfiles(int code);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
        [OperationContract]
        void SendPlayerProfiles(List<string> playerProfiles);
    }
}