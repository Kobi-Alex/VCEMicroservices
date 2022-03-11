using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Infrastructure.Persistance.Idempotency;


namespace Report.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class ClientRequestEntityTypeConfiguration
        : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> builder)
        {
            builder.ToTable("requests", ReportDbContext.DEFAULT_SCHEMA);
            builder.Property(cr => cr.Id).ValueGeneratedNever();
            builder.Property(cr => cr.Name).IsRequired();
            builder.Property(cr => cr.Time).IsRequired();
        }
    }
}
