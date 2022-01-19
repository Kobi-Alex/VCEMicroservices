using System;
using AutoMapper;
using MassTransit;
using System.Threading.Tasks;
using Exam.API.Application.IntegrationEvents.Events;
using Exam.Domain.Repositories;
using Exam.Domain.Entities;
using System.Linq;

namespace Exam.API.Application.IntegrationEvents
{
    public class ExamIntegrationEventService : IExamIntegrationEventService, IConsumer<QuestionItemDeleteEvent>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ExamIntegrationEventService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        }


        public async Task Consume(ConsumeContext<QuestionItemDeleteEvent> context)
        {

            if (context is null)
            {
                throw new ConsumerMessageException();
            }

            var exams = await _repositoryManager.ExamItemRepository.GetAllAsync();

            var questions = exams.SelectMany(ex => ex.ExamQuestions, (ex, qu) => new { exam = ex, question = qu })
                .Where(ex => ex.exam.Status == ExamStatus.NotAvailable && ex.question.QuestionItemId == context.Message.Id)
                .Select(ex => ex.question);


            foreach (var item in questions)
            {
                _repositoryManager.ExamQuestionRepository.Remove(item);
            }

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }

        public Task SaveEventAndCatalogContextChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}