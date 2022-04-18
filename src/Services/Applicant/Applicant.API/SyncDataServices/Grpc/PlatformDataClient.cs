using Applicant.Domain.Entities;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private IConfiguration _configuration;

        public PlatformDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IEnumerable<User> ReturnAllPlatforms()
        {
            Console.WriteLine($"---> calling GRPC Service: {_configuration["GrpcPlatform:Plat"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform:Plat"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }



        }
    }
}
