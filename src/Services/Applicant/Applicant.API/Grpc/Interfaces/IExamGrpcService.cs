using GrpcExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Grpc.Interfaces
{
    public interface IExamGrpcService
    {
        public ExamItemModel GetExamItem(int idExam);
        public ExamQuestionsResponse GetExamQuestions(int id);
    }
}
