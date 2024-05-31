using ASSISTENTE.Domain.Entities.Questions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration.Configuration
{
    internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Text).IsRequired();

            builder.HasMany(e => e.Resources)
                .WithOne(e => e.Question)
                .HasForeignKey(e => e.QuestionId);
            
            builder.HasMany(e => e.Files)
                .WithOne(e => e.Question)
                .HasForeignKey(e => e.QuestionId);
        }
    }
}