using ASSISTENTE.Domain.Entities.Answers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Text).IsRequired();
            builder.Property(e => e.Prompt).IsRequired();
            builder.Property(e => e.Client).IsRequired();
            builder.Property(e => e.Model).IsRequired();
            builder.Property(e => e.PromptTokens).IsRequired();
            builder.Property(e => e.CompletionTokens).IsRequired();

            builder.HasOne(e => e.Question)
                .WithOne(e => e.Answer)
                .HasForeignKey<Answer>(e => e.QuestionId);
        }
    }
}