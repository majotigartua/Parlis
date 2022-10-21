using Parlis.Server.DataAccess;
using System.ServiceModel;

namespace Parlis.Server.Service
{
    [ServiceContract(CallbackContract = typeof(IMatchManagementCallback))]
    public interface IMatchManagement
    {
        [OperationContract(IsOneWay = true)]
        void Connect(PlayerProfile playerProfile);
    }

    [ServiceContract]
    public interface IMatchManagementCallback
    {
    }
}