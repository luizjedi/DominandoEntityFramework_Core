using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo_de_Dados.Domain;

namespace Modelo_de_Dados.Configurations
{
    public class AtorConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            builder.HasMany(x => x.Filmes)
                   .WithMany(x => x.Atores)
                   .UsingEntity(p => p.ToTable("AtoresFilmes"));

            builder.Navigation(p => p.Filmes).AutoInclude(); // faz alto include quando necessário
        }
    }
}