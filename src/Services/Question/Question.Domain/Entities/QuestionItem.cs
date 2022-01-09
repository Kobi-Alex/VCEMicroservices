using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Entities
{
    public class QuestionItem
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }


        // Foreign Key
        public int QuestionCategoryId { get; set; }
        // Navigation property
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}