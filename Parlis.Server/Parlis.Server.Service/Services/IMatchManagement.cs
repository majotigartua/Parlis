using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IMatchManagementCallback))]
    public interface IMatchManagement
    {
        [OperationContract]
        bool CheckMatchExistence(int code);

        [OperationContract]
        void CreateMatch(int code);

        [OperationContract(IsOneWay = false)]
        void JoinMatch(string username, int code);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
    }
}