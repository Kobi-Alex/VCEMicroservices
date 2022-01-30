using Report.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.AggregatesModel.ReviewAggregate
{
    public class Review : Entity, IAggregateRoot
    {

        private int _examId;                                   // ID іспиту
        private string _applicantId;                           // ID абітурієнта(userId)
        private string _description;                           // опис звіту
        private DateTime _reportDate;                          // дата звіту
        private readonly List<QuestionUnit> _questionUnits;    // список питань екзамену


        public int GetExamId => _examId;
        public string GetApplicantId => _applicantId;
        public IReadOnlyCollection<QuestionUnit> QuestionUnits => _questionUnits;


        protected Review()
        {
            _questionUnits = new List<QuestionUnit>();
        }

        public Review(string description, string applicantId = null) : this()
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            _reportDate = DateTime.UtcNow;
        }


        public void SetExamId(int id)
        {
            _examId = id;
        }

        public void SetApplicantId(string id)
        {
            _applicantId = id;
        } 

        public void AddQuestionUnit(int questionId, string questionName, string answerKeys, string currentKeys, int totalNumberAnswer)
        {
            var existingReportForQuestion = _questionUnits
                .Where(o => o.QuestionId == questionId)
                .SingleOrDefault();

            if (existingReportForQuestion != null)
            {
                if (!String.Equals(existingReportForQuestion.GetCurrentKeys, currentKeys))
                {
                    existingReportForQuestion.SetCurrentAnswer(currentKeys);
                }
            }
            else
            {

                if (questionId <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(questionId));
                }

                if (string.IsNullOrEmpty(questionName))
                {
                    throw new ArgumentNullException(nameof(questionName));
                }

                if (totalNumberAnswer <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(questionId));
                }

                var questionUnit = new QuestionUnit(questionId, questionName, answerKeys, currentKeys, totalNumberAnswer);
                _questionUnits.Add(questionUnit);
            }

  
        }


        public decimal GetTotalScore()
        {
            //TODO add logic total score.. -> загальний підрахунок правильних відповідей
            return 0.0m;
        }

        public decimal GetGrade()
        {
            //TODO add logic total grade of exam.. (A, B, C, D, E)
            return 0.0m;
            //return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
        }

    }
}
