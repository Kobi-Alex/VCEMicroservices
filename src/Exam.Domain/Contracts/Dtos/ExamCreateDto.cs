using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.Domain.Contracts.Dtos
{
    public class ExamCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(60, ErrorMessage = "Title can't be longer than 60 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(60, ErrorMessage = "Category can't be longer than 60 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description can't be longer than 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration time of exam is required")]
        public int DurationTime { get; set; }

        [Required(ErrorMessage = "Passing score of exam is required")]
        public decimal PassingScore { get; set; }

        [Required(ErrorMessage = "Date of exam is required")]
        public DateTime DateExam { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(60, ErrorMessage = "Status can't be longer than 60 characters")]
        public string Status { get; set; }
    }
}