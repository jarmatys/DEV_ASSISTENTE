using ASSISTENTE.Domain.Entities.Answers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Text).IsRequired();
            builder.Property(e => e.Prompt).IsRequired();

            builder.ComplexProperty(property => property.Metadata, metadata =>
            {
                metadata.Property(e => e.Client).HasColumnName("Client").IsRequired();
                metadata.Property(e => e.Model).HasColumnName("Model").IsRequired();
                metadata.Property(e => e.PromptTokens).HasColumnName("PromptTokens").IsRequired();
                metadata.Property(e => e.CompletionTokens).HasColumnName("CompletionTokens").IsRequired();
            });

            builder.HasOne(e => e.Question)
                .WithOne(e => e.Answer)
                .HasForeignKey<Answer>(e => e.QuestionId);
        }
    }
}