using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class CompteBanqueConfiguration : IEntityTypeConfiguration<CompteBanque>
    {
        public void Configure(EntityTypeBuilder<CompteBanque> builder)
        {
            builder.Property(c => c.Solde)
                .HasPrecision(18, 2);
        }
    }
}
