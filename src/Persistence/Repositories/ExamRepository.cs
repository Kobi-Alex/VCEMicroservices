using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class ExamRepository : GenericRepository<ExamItem>, IExamRepository
    {
        public ExamRepository(ExamDbContext context) : base(context)
        {
        }
    }
}
