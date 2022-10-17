using Parlis.Server.DataAccess;
using System.ServiceModel;

namespace Parlis.Server.BusinessLogic
{
    [ServiceContract]
    public interface IPlayerProfileManagement
    {
        [OperationContract]
        bool CheckPlayerExistence(Player player);

        [OperationContract]
        bool CheckPlayerProfileExistence(PlayerProfile playerProfile);

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        bool RegisterPlayer(Player player);

        [OperationContract]
        bool RegisterPlayerProfile(PlayerProfile playerProfile);
    }
}