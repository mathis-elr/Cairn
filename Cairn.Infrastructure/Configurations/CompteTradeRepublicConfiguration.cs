using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cairn.Domain.Modeles;
using Microsoft.EntityFrameworkCore;

namespace Cairn.Infrastructure.Configurations
{
    internal class CompteTradeRepublicConfiguration : IEntityTypeConfiguration<CompteTradeRepublic>
    {
        public void Configure(EntityTypeBuilder<CompteTradeRepublic> builder)
        {
        }
    }
}
