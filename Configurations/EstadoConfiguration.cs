using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo_de_Dados.Domain;

namespace Modelo_de_Dados.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.HasOne(x => x.Governador)
                   .WithOne(x => x.Estado)
                   .HasForeignKey<Governador>(x => x.EstadoId);

            builder.HasMany(x => x.Cidades)
                   .WithOne(x => x.Estado);

            builder.Navigation(p => p.Governador).AutoInclude(); // faz alto include quando necessário
            builder.Navigation(p => p.Cidades).AutoInclude(); // faz alto include quando necessário
        }
    }
}