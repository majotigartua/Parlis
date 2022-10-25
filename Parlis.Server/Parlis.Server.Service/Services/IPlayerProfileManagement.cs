using Parlis.Server.Service.Data;
using System.ServiceModel;

namespace Parlis.Server.Service.Services
{
    [ServiceContract]
    public interface IPlayerProfileManagement
    {
        [OperationContract]
        bool CheckPlayerExistence(string emailAddress);

        [OperationContract]
        bool CheckPlayerProfileExistence(string username);

        [OperationContract]
        bool DeletePlayer(string emailAddress);

        [OperationContract]
        bool DeletePlayerProfile(string username);

        [OperationContract]
        Player GetPlayer(string username);

        [OperationContract]
        PlayerProfile GetPlayerProfile(string emailAddress);

        [OperationContract]
        PlayerProfile Login(string username, string password);

        [OperationContract]
        bool RegisterPlayer(Player player);

        [OperationContract]
        bool RegisterPlayerProfile(PlayerProfile playerProfile);

        [OperationContract]
        bool SendMail(string username, string title, string message, int code);

        [OperationContract]
        bool UpdatePlayer(Player player);

        [OperationContract]
        bool UpdatePlayerProfile(PlayerProfile playerProfile);
    }
}