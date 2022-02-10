using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.API.Application.Contracts.ExamItemDtos
{
    public class ExamItemUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description can't be longer than 1000 characters")]
        public string Description { get; set; }

        [Range(30.0, 360, ErrorMessage = "In range from 30 to 240 minutes")]
        public int DurationTime { get; set; }

        [Range(40.0, 100.0, ErrorMessage = "In range from 40 to 100 mark")]
        public decimal PassingScore { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime DateExam { get; set; }

    }
}