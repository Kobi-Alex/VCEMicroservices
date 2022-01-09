﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Entities
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public decimal CorrectAnswerCoefficient { get; set; }

        // Foreign Key
        public int QuestionItemId { get; set; }
        // Navigation property
        public virtual QuestionItem QuestionItem { get; set; }
    }
}