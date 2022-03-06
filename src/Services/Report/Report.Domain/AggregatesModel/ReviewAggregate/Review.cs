﻿using System;
using System.Linq;
using Report.Domain.SeedWork;
using System.Collections.Generic;

namespace Report.Domain.AggregatesModel.ReviewAggregate
{
    public class Review : Entity, IAggregateRoot
    {

        public int _examId;                                    // ID exam
        public string _applicantId;                            // ID applicant (userId)
        private decimal _totalScore;                           // Count of correct answers
        private decimal _persentScore;                         // Count of correct answers in %
        private string _grade;                                 // Grade
        private DateTime _reportDate;                          // Date of report
        private readonly List<QuestionUnit> _questionUnits;    // Exam answer list answered by applicant

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


        /// <summary>
        /// Adding new or update current answer 
        /// </summary>
        /// <param name="questionName"></param>
        /// <param name="answerKeys"></param>
        /// <param name="currentKeys"></param>
        /// <param name="totalNumberAnswer"></param>
        /// <param name="questionId"></param>
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

                var questionUnit = new QuestionUnit(questionName, answerKeys, currentKeys, 
                    totalNumberAnswer, questionId);
                _questionUnits.Add(questionUnit);

            }
        }


        /// <summary>
        /// Calculating score by report
        /// </summary>
        public void CalculateScores()
        {
            int totalScore = 0;

            foreach (var item in _questionUnits)
            {
                if (item.GetTotalNumberAnswer == 1)
                {
                    double totalCorrectAnswer = 0;
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                    string[] keyAnswerWords = item.GetAnswerKeys.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] currentAnswerWords = item.GetCurrentKeys.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in currentAnswerWords)
                    {
                        if (!item.GetAnswerKeys.Contains(word))
                        {
                            return;
                        }
                        totalCorrectAnswer++;
                    }

                    if ((totalCorrectAnswer / keyAnswerWords.Length) > 0.7)
                    {
                        totalScore++;
                    }
                }
                else
                {
                    if(item.GetTotalNumberAnswer > 1)
                    {
                        if(item.GetCurrentKeys.Equals(item.GetAnswerKeys))
                        {
                            totalScore++;
                        }
                    }
                }
            }

            _totalScore = totalScore;
            _persentScore = (totalScore * 100) / _questionUnits.Count;
            _grade = GetGradeByPersentScore();
        }


        /// <summary>
        /// Get a grade depending on the score
        /// </summary>
        /// <returns> Return value of grade </returns>
        public string GetGradeByPersentScore()
        {
            if (_persentScore > 89) return "A";
            if (_persentScore > 74) return "B";
            if (_persentScore > 59) return "C";
                                    return "F";
        }

    }
}