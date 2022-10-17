using Parlis.Server.BusinessLogic;
using System;
using System.ServiceModel;

namespace Parlis.Server
{
    public class Server
    {
        public static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(PlayerProfileManagement)))
            {
                host.Open();
                Console.WriteLine("Server is running...");
                Console.ReadLine();
            }
        }
    }
}