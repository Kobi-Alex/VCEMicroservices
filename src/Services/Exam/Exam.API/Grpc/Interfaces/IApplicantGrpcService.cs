using GrpcApplicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc.Interfaces
{
    public interface IApplicantGrpcService
    {
        public UserExamResponse CheckIfExamExistsInUsers(int examId);
    }
}
