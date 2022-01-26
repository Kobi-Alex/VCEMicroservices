using Report.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.AggregatesModel.ReportAggregate
{
    public class Report : Entity, IAggregateRoot
    {

        private string _titleExam;                              // назва іспиту
        private string _descriptionExam;                        // опис іспиту
        private DateTime _reportDate;                           // дата звіту
        private int? _applicantId;                              // ID абітурієнта
        private readonly List<ReportItem> _reportItems;         // Список відповідей


        public int? GetApplicantId => _applicantId;
        public IReadOnlyCollection<ReportItem> ReportItems => _reportItems;


        protected Report()
        {
            _reportItems = new List<ReportItem>();
        }

        public Report(string titleExam, string descriptionExam, int? applicantId = null) : this()
        {
            _titleExam = titleExam;
            _descriptionExam = descriptionExam;
            _reportDate = DateTime.UtcNow;
        }


        public void SetApplicantId(int id)
        {
            _applicantId = id;
        }

        public void AddReportItem(int questionId, int units, string questionName, List<string> answerKeys, List<string> currentKeys)
        {
            var existingReportForQuestion = _reportItems
                .Where(o => o.QuestionId == questionId)
                .SingleOrDefault();

            if (existingReportForQuestion != null)
            {
                if (currentKeys.Count > 0)
                {
                    existingReportForQuestion.SetCurrentAnswer(currentKeys);
                }
            }
            else
            {
                //TODO add validated new report item

                var reportItem = new ReportItem(questionId, units, questionName, answerKeys, currentKeys);
                _reportItems.Add(reportItem);
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
