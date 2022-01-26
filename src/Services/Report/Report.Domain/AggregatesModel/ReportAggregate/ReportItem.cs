using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Report.Domain.SeedWork;

namespace Report.Domain.AggregatesModel.ReportAggregate
{
    public class ReportItem : Entity
    {
        private int _units;                             // загальна к-сть відповідей у питанні
        private string _questionName;                   // саме питання
        private readonly List<string> _answerKeys;      // правильна відповідь [A, B, C, D, E]
        private readonly List<string> _currentKeys;     // поточна відповідь абітурієнта [A, B, C, D, E]

        public int QuestionId { get; private set; }     // Id поточного питання
        

        protected ReportItem() 
        { 
        }

        public ReportItem(int questionId, int units, string questionName, List<string> answerKeys, List<string> currentKeys)
        {
            QuestionId = questionId;
            _units = units;
            _questionName = questionName;
            _answerKeys = answerKeys;
            _currentKeys = currentKeys;
        }


        public int GetUnits() => _units;
        public string GetQuestionName() => _questionName;
        public IReadOnlyCollection<string> AnswerKeys => _answerKeys;
        public IReadOnlyCollection<string> CurrentKeys => _currentKeys;


        public void SetCurrentAnswer(List<string> keys)
        {
            if (_currentKeys.Count != 0)
            {
                _currentKeys.Clear();
            }
            _currentKeys.AddRange(keys);
        }
    }
}
