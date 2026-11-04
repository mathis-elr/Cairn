using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class CompteBanqueConfiguration : IEntityTypeConfiguration<CompteBanque>
    {
        public void Configure(EntityTypeBuilder<CompteBanque> builder)
        {
            builder.HasIndex(c => c.IdPowens)
                .IsUnique();

            builder.HasOne(c => c.Banque)
                .WithMany(b => b.ComptesBanques)
                .HasForeignKey(c => c.BanqueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
