using System;
using System.Linq;
using Report.Domain.SeedWork;


namespace Report.Domain.AggregatesModel.ReviewAggregate
{
    public class QuestionUnit : Entity
    {
        //private String _name;                       // the same question name
        private String _answerKeys;                   // correct answer from QuestionService (Char: A, B, C ...)
        private String _currentKeys;                  // current answer from applcant (Char: A,B or A, or Text answer..)
        private int _totalNumberAnswer;               // total answers count in question

        public int QuestionId { get; private set; }   // Id current question

        public int ReviewId { get; set; }
        public Review Review { get; set; }

        protected QuestionUnit() 
        {
        }

        public QuestionUnit( string answerKeys, string currentKeys, 
            int totalNumberAnswer, int questionId) : this()
        {
            if (questionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(questionId));
            }

            //if (string.IsNullOrEmpty(questionName))
            //{
            //    throw new ArgumentNullException(nameof(questionName));
            //}


            //_name = questionName;
            _answerKeys = answerKeys;
            _currentKeys = currentKeys;
            _totalNumberAnswer = totalNumberAnswer;
            QuestionId = questionId;
        }


        //public string GetQuestionName() => _name;
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
