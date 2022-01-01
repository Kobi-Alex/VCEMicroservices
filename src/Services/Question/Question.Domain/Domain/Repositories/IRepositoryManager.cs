using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IQuestionItemRepository QuestionRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
