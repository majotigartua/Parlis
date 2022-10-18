using Parlis.Server.DataAccess;
using System.ServiceModel;

namespace Parlis.Server.Service
{
    [ServiceContract]
    public interface IPlayerProfileManagement
    {
        [OperationContract]
        bool CheckPlayerExistence(Player player);

        [OperationContract]
        bool CheckPlayerProfileExistence(PlayerProfile playerProfile);

        [OperationContract]
        bool Login(PlayerProfile playerProfile);

        [OperationContract]
        bool RegisterPlayer(Player player);
    }
}