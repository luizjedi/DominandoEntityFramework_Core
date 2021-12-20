using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo_de_Dados.Domain;

namespace Modelo_de_Dados.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.OwnsOne(x => x.Endereco, end =>
           {
               end.Property(x => x.Bairro).HasColumnName("Bairro"); // Personaliza o nome a ser criado na tabela do banco de dados
               end.Property(x => x.Logradouro).HasColumnName("Logradouro");
               end.Property(x => x.Cidade).HasColumnName("Cidade");
               end.Property(x => x.Estado).HasColumnName("Estado");

               end.ToTable("Endereco");
           });
        }
    }
}