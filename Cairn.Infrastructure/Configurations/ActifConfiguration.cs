using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class ActifConfiguration : IEntityTypeConfiguration<Actif>
    {
        public void Configure(EntityTypeBuilder<Actif> builder)
        {
            builder.HasIndex(e => e.IdPowens)
                .IsUnique();
        }
    }
}
