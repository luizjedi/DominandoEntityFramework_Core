using System;
using System.IO;
using Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.Data
{
    public class ApplicationContext : DbContext
    {
        // Propriedade append evita a criação de novos arquivos, aproveitando o arquivo já criado e atualiza seus logs.
        private readonly StreamWriter _writer = new StreamWriter("Meu_Log_do_EF_Core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Infraestrutura;Integrated Security=true;pooling=true";

            optionsBuilder
                .UseSqlServer(
                    strConnection,
                        x => x
                            .MaxBatchSize(100) // Batch Size permite personalizar
                            .CommandTimeout(7)
                            .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)) // Habilita a resiliência da aplicação
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

        }

        // Realiza o flush dos dados no arquivo de log
        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}