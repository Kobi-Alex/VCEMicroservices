using GrpcApplicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Grpc.Interfaces
{
    public interface IApplicantGrpcService
    {
        public RemoveExamResponce RemoveExamFromApplicantData(string userId, int examId);
        public GetUserDataResponse GetUserData(string userId);
    }
}
