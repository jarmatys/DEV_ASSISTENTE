using ASSISTENTE.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASSISTENTE.Persistence.Configuration
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Content).IsRequired();
            
            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.Type).HasConversion<string>();
        }
    }
}