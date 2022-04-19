using GrpcReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Grpc.Interfaces
{
    public interface IReportGrpcService
    {
        public UserDataResponse RemoveUserDataFromReport(string userId);
        public IsExistExamResponse IsExistExamRequest(string userId, int examId);
    }
}
