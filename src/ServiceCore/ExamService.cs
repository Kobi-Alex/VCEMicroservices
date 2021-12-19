using Contracts;
using ServicesCore.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCore
{
    internal sealed class ExamService : IExamService
    {

        public void CreateExam(ExamCreateDto exam)
        {
            throw new NotImplementedException();
        }

        public void DeleteExam(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExamReadDto> GetExamByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public ExamReadDto GetExamById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExamReadDto> GetExamByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExamReadDto> GetExams()
        {
            throw new NotImplementedException();
        }

        public void UpdateExam(ExamUpdateDto exam)
        {
            throw new NotImplementedException();
        }
    }
}
