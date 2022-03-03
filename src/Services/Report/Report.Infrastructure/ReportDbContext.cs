using System.Threading;
using System.Threading.Tasks;
using Report.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Report.Domain.AggregatesModel.ReviewAggregate;

namespace Report.Infrastructure
{
    public sealed class ReportDbContext : DbContext, IUnitOfWork
    {

        public const string DEFAULT_SCHEMA = "report";
        public DbSet<Review> Reviews { get; set; }
        public DbSet<QuestionUnit> QuestionUnits { get; set; }


        public ReportDbContext(DbContextOptions<ReportDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportDbContext).Assembly);


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
