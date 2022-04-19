using GrpcQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Grpc.Interfaces
{
    public interface IQuestionGrpcService
    {
        public QuestionUnitModel GetQuestionUnitFromQuestionData(int questionId);
    }
}
