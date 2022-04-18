using Applicant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<User> ReturnAllPlatforms();
    }
}
