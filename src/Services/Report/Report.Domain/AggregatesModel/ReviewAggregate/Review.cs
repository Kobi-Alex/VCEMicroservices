﻿using Report.Domain.SeedWork;
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
        public decimal _totalScore;                            // K-сть правельних відповідей
        public decimal _persentScore;                          // K-сть правельних відповідей у відсотках(%)
        public string _grade;                                  // Оцінка за іспит
        private DateTime _reportDate;                          // дата звіту
        private readonly List<QuestionUnit> _questionUnits;    // список питань екзамену на які відповів абітурієнт


        public int GetExamId => _examId;
        public string GetApplicantId => _applicantId;
        public IReadOnlyCollection<QuestionUnit> QuestionUnits => _questionUnits;


        protected Review()
        {
            _questionUnits = new List<QuestionUnit>();  
        }

        public Review(int examId, string applicantId, decimal totalScore = 0 , decimal persentScore = 0) 
            : this()
        {
            if(examId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(examId));
            }

            if (string.IsNullOrEmpty(applicantId))
            {
                throw new ArgumentNullException(nameof(applicantId));
            }

            _examId = examId;
            _applicantId = applicantId;
            _totalScore = totalScore;
            _persentScore = persentScore;
            _reportDate = DateTime.UtcNow;
        }


        public void SetGrade(string grade)
        {
            _grade = grade;
        }

        public void SetTotalScore(decimal totalScore)
        {
            _totalScore = totalScore;
        }

        public void SetPersentScore(decimal persentScore)
        {
            _persentScore = persentScore;
        }

        //public void SetExamId(int id)
        //{
        //    _examId = id;
        //}

        //public void SetApplicantId(string id)
        //{
        //    _applicantId = id;
        //}

        public void AddQuestionUnit(string questionName, string answerKeys, string currentKeys, 
            int totalNumberAnswer, int questionId)
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

                var questionUnit = new QuestionUnit( questionName, answerKeys, currentKeys, 
                    totalNumberAnswer, questionId);
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
