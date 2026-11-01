using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class PositionInvestissementConfiguration : IEntityTypeConfiguration<PositionInvestissement>
    {
        public void Configure(EntityTypeBuilder<PositionInvestissement> builder)
        {
            builder.Property(e => e.Quantite)
                .HasPrecision(18, 6);

            builder.Property(e => e.PrixAchat)
                .HasPrecision(18, 4);

            builder.Property(e => e.PrixCourant)
                .HasPrecision(18, 4);
        }
    }
}
