using GrpcExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Grpc.Interfaces
{
    public interface IExamGrpcService
    {
        public  ExamItemModel GetExamItemFromExamData(int examId);
        public  ExamResponse CheckIfQuestionExistsInExam(int questionId);
    }
}
