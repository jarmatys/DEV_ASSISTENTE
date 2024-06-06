using ASSISTENTE.Domain.Entities.QuestionNotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration.Configuration
{
    internal class QuestionNoteConfiguration : IEntityTypeConfiguration<QuestionNote>
    {
        public void Configure(EntityTypeBuilder<QuestionNote> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Question).WithOne(e => e.NoteContext);

            builder.Property(e => e.QuestionId).IsRequired();
        }
    }
}