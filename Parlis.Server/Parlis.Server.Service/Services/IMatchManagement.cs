using Parlis.Server.Service.Data;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
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