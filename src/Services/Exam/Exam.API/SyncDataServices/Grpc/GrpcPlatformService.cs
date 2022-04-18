using Grpc.Core;
using PlatformService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.SyncDataServices.Grpc
{
    public class GrpcPlatformService: GrpcPlatform.GrpcPlatformBase
    {
        public GrpcPlatformService()
        {

        }

        public override  Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            response.Platform.Add(new GrpcPlatformModel() { Name = "Hello", PlatformId = 1 });


            return Task.FromResult(response);
        }
    }
}
