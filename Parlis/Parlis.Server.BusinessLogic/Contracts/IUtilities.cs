using System.ServiceModel;

namespace Parlis.Server.BusinessLogic
{
    [ServiceContract]
    public interface IUtilities
    {
        [OperationContract]
        string ComputeSHA256Hash(string password);

        [OperationContract]
        bool ValidatePasswordFormat(string password);
    }
}