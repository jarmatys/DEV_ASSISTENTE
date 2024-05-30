using ASSISTENTE.Domain.Entities.Questions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration.Configuration
{
    public class QuestionResourceConfiguration : IEntityTypeConfiguration<QuestionResource>
    {
        public void Configure(EntityTypeBuilder<QuestionResource> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ResourceId).IsRequired();
            
            builder.Property(e => e.QuestionId).IsRequired();
        }
    }
}