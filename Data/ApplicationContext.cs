using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using UDFs.Domain;
using UDFs.Functions;

namespace UDFs.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_UDFs;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }


        // Fará a conexão do ApplicationContext com os métodos criados em MyFunctions.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UDFs.Functions.MyFunctions.FunctionsRegister(modelBuilder);

            // Registrando funções com fluente API
            modelBuilder
                .HasDbFunction(_myFunction)
                .HasName("LEFT")
                .IsBuiltIn();

            modelBuilder
                .HasDbFunction(_letrasMaiusculas)
                .HasName("ConverterParaLetrasMaiusculas")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(_dateDIFF)
                .HasName("DATEDIFF")
                // Traduzir o valor do identificador sem aspas para o banco de dados.
                .HasTranslation(p =>
                {
                    var argumentos = p.ToList();
                    var constante = (SqlConstantExpression)argumentos[0];
                    argumentos[0] = new SqlFragmentExpression(constante.Value.ToString());

                    return new SqlFunctionExpression("DATEDIFF", argumentos, false, new[] { false, false, false }, typeof(int), null);
                })
                .IsBuiltIn();
        }

        // 1ª Forma:
        private static MethodInfo _myFunction = typeof(MyFunctions)
                .GetRuntimeMethod("Left", new[] { typeof(string), typeof(int) });
        // 2ª Forma:
        private static MethodInfo _letrasMaiusculas = typeof(MyFunctions)
                .GetRuntimeMethod(nameof(MyFunctions.LetrasMaiusculas), new[] { typeof(string) });

        private static MethodInfo _dateDIFF = typeof(MyFunctions)
                .GetRuntimeMethod(nameof(MyFunctions.DateDIFF), new[] { typeof(string), typeof(DateTime), typeof(DateTime) });

        // [DbFunction(name: "LEFT", IsBuiltIn = true)]
        // public static string Left(string dados, int quantidade)
        // {
        //     throw new NotImplementedException();
        // }
    }
}