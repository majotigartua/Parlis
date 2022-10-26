using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatManagement
    {
    }

    [ServiceContract]
    public interface IChatCallback
    {
    }
}