using ASSISTENTE.Domain.Entities.QuestionCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration.Configuration
{
    internal class QuestionCodeConfiguration : IEntityTypeConfiguration<QuestionCode>
    {
        public void Configure(EntityTypeBuilder<QuestionCode> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Question).WithOne(e => e.CodeContext);

            builder.Property(e => e.QuestionId).IsRequired();
        }
    }
}