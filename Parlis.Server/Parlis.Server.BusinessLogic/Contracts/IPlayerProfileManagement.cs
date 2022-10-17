using Parlis.Server.DataAccess;
using System.ServiceModel;

namespace Parlis.Server.BusinessLogic
{
    [ServiceContract]
    public interface IPlayerProfileManagement
    {
        [OperationContract]
        bool CheckPlayerExistence(string emailAddress);

        [OperationContract]
        bool CheckPlayerProfileExistence(string username);

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        bool RegisterPlayer(Player player);
    }
}