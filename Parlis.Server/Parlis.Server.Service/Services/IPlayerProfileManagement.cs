using Parlis.Server.Service.Data;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract]
    public interface IPlayerProfileManagement
    {
        [OperationContract]
        bool CheckPlayerExistence(Player player);

        [OperationContract]
        bool CheckPlayerProfileExistence(PlayerProfile playerProfile);

        [OperationContract]
        bool DeletePlayer(Player player);

        [OperationContract]
        bool DeletePlayerProfile(PlayerProfile playerProfile);

        [OperationContract]
        Player GetPlayer(PlayerProfile playerProfile);

        [OperationContract]
        PlayerProfile Login(string username, string password);

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