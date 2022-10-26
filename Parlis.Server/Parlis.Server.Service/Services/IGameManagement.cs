using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IGameManagementCallback))]
    public interface IGameManagement
    {
    }

    [ServiceContract]
    public interface IGameManagementCallback
    {
    }
}