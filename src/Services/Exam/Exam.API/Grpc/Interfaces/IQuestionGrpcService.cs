using GrpcQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc.Interfaces
{
    public interface IQuestionGrpcService
    {
        public QuestionUnitModel GetQuestionById(int id);
    }
}
