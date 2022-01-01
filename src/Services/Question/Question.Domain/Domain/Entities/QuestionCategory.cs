using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Entities
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public int QuestionItemId { get; set; }
        public QuestionItem QuestionItem { get; set; }
    }
}