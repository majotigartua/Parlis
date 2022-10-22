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
        Player GetPlayer(PlayerProfile playerProfile);

        [OperationContract]
        bool Login(PlayerProfile playerProfile);

        [OperationContract]
        bool RegisterPlayer(Player player);

        [OperationContract]
        bool RegisterPlayerProfile(PlayerProfile playerProfile);

        [OperationContract]
        bool SendMail(PlayerProfile playerProfile, string title, string message, int code);

        [OperationContract]
        bool UpdatePlayer(Player player);

        [OperationContract]
        bool UpdatePlayerProfile(PlayerProfile playerProfile);
    }
}