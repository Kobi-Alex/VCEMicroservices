using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Entities
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<QuestionItem> QuestionItems { get; set; }
    }
}