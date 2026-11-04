using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cairn.Infrastructure.Configurations
{
    internal class CoursActifConfiguration : IEntityTypeConfiguration<CoursActif>
    {
        public void Configure(EntityTypeBuilder<CoursActif> builder)
        {
            builder.HasKey(c => new { c.ActifId, c.Date });

            builder.Property(c => c.Prix)
                .HasPrecision(18, 6);

            builder.HasOne(c => c.Actif)
                .WithMany(a => a.CoursActif)
                .HasForeignKey(c => c.ActifId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
