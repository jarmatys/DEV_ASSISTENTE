using ASSISTENTE.Domain.Entities.Questions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration
{
    public class QuestionFileConfiguration : IEntityTypeConfiguration<QuestionFile>
    {
        public void Configure(EntityTypeBuilder<QuestionFile> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Text).IsRequired();
            
            builder.Property(e => e.QuestionId).IsRequired();
        }
    }
}