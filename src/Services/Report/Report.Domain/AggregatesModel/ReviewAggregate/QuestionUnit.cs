using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Report.Domain.SeedWork;

namespace Report.Domain.AggregatesModel.ReviewAggregate
{
    public class QuestionUnit : Entity
    {
        private String _name;                         // саме питання
        private String _answerKeys;                   // правильна відповідь [C, D, E]
        private String _currentKeys;                  // поточна відповідь абітурієнта [A, B, C, D, E]
        private int _totalNumberAnswer;               // загальна к-сть відповідей у питанні

        public int QuestionId { get; private set; }   // Id поточного питання


        protected QuestionUnit() 
        {
        }

        public QuestionUnit( string questionName, string answerKeys, string currentKeys, 
            int totalNumberAnswer, int questionId) : this()
        {
            if (questionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(questionId));
            }


            if (string.IsNullOrEmpty(questionName))
            {
                throw new ArgumentNullException(nameof(questionName));
            }


            _name = questionName;
            _answerKeys = answerKeys;
            _currentKeys = currentKeys;
            _totalNumberAnswer = totalNumberAnswer;
            QuestionId = questionId;
        }


        public string GetQuestionName() => _name;
        public string GetAnswerKeys => _answerKeys;
        public string GetCurrentKeys => _currentKeys;
        public int GetTotalNumberAnswer => _totalNumberAnswer;


        public void SetCurrentAnswer(string keys)
        {
            if (keys.Any(c => char.IsLetter(c)))
            {
                _currentKeys = keys;
            }
        }

    }
}
