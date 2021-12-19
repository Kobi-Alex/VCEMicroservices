using Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCore.Abstractions.Interfaces
{
    public interface IExamService
    {
        IEnumerable<ExamReadDto> GetExams();
        ExamReadDto GetExamById(int id);
        IEnumerable<ExamReadDto> GetExamByTitle(string title);
        IEnumerable<ExamReadDto> GetExamByCategory(string category);

        void UpdateExam(ExamUpdateDto exam);
        void CreateExam(ExamCreateDto exam);
        void DeleteExam(int Id);
    }
}
