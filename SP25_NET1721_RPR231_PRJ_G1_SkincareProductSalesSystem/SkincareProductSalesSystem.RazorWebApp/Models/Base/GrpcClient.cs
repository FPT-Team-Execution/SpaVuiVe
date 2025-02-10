using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Common
{

    public class GrpcClient<T> where T : class
    {
        public T Client { get; }

        public GrpcClient(IConfiguration configuration)
        {
            var grpcAddress = configuration.GetValue<string>("Grpc:ServerAddress")!;
            /*
             "Grpc": {
                "ServerAddress": "your-domain-server"
             }
             */
            var channel = GrpcChannel.ForAddress(grpcAddress);
            Client ??= (T)Activator.CreateInstance(typeof(T), channel)!;
        }
    }

}
