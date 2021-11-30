using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DominandoEFCore
{
    public class Program
    {
        static int _count;
        public static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            // GapDoEnsureCreated();
            // HealthCheckBancoDeDados();

            // warmup
            new Curso.Data.ApplicationContext().Departamentos.AsNoTracking().Any();
            _count = 0;
            GerenciarEstadoDaConexão(false);
            _count = 0;
            GerenciarEstadoDaConexão(true);
        }

        //Gerencia o estado da conexão
        static void GerenciarEstadoDaConexão(bool gerenciarEstadoDaConexão)
        {
            using var db = new Curso.Data.ApplicationContext();
            var time = System.Diagnostics.Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();
            conexao.StateChange += (_, __) => ++_count; // Realiza a contagem das mudanças de estado da conexão.
            if (gerenciarEstadoDaConexão)
            {
                conexao.Open();
            }
            for (var i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();
            // Elapsed.ToString() mostra o tempo formatado até a casa de milisegundos.
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoDaConexão}, Contador: {_count}";

            Console.WriteLine(mensagem);
        }
        // Checar a conexão com o banco de dados
        static void HealthCheckBancoDeDados()
        {
            using var db = new Curso.Data.ApplicationContext();
            var canConnect = db.Database.CanConnect();

            if (canConnect)
            {
                Console.WriteLine("Posso me conectar");
            }
            else
            {
                Console.WriteLine("Não posso me conectar");
            }
        }
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Curso.Data.ApplicationContext();
            db.Database.EnsureCreated();
            // db.Database.EnsureDeleted();

        }

        static void GapDoEnsureCreated()
        {
            using var db1 = new Curso.Data.ApplicationContext();
            using var db2 = new Curso.Data.ApplicationContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            // Force a criação do segundo contexto, resolvendo o problema de GAP na criação do banco de dados.
            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }
    }
}
