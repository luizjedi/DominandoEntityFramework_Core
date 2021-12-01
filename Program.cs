using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
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
            // new Curso.Data.ApplicationContext().Departamentos.AsNoTracking().Any();
            // _count = 0;
            // GerenciarEstadoDaConexão(false);
            // _count = 0;
            // GerenciarEstadoDaConexão(true);
            // ExecuteSql();
            // SqlInjection();
            // MigracoesPendentes();
            // AplicarMigracoesEmTempoDeExecucao();
            // TodasMigracoes();
            // MigracoesJaAplicadas();
            // ScriptGeralDoBancoDeDados();
            // CarregamentoAdiantado();
            CarregamentoExplicito();
        }
        // Carregamento Explícito
        static void CarregamentoExplicito()
        {
            using var db = new Curso.Data.ApplicationContext();
            Curso.Scripts.TiposDeCarregamento.SetupTiposDeCarregamento(db);

            var departamentos = db
                .Departamentos
                .ToList(); // Ao invés de ToList() pode ser adicionado na string de conexão o seguinte parâmetro: MultipleActiveResultSets=true

            foreach (var departamento in departamentos)
            {
                if (departamento.Id == 2)
                {
                    // db.Entry(departamento).Collection(p => p.Funcionarios).Load();
                    db.Entry(departamento).Collection(x => x.Funcionarios).Query().Where(x => x.Id > 2).ToList();
                }

                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionário: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionário encontrado!");
                }
            }
        }
        // Carregamento Adiantado (Eager)
        static void CarregamentoAdiantado()
        {
            using var db = new Curso.Data.ApplicationContext();
            Curso.Scripts.TiposDeCarregamento.SetupTiposDeCarregamento(db);

            var departamentos = db
                .Departamentos
                .Include(p => p.Funcionarios);

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionário: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionário encontrado!");
                }
            }
        }
        // Gerando o script geral do banco de dados
        static void ScriptGeralDoBancoDeDados()
        {
            using var db = new Curso.Data.ApplicationContext();

            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }
        // Recuperando as migrações já aplicadas
        static void MigracoesJaAplicadas()
        {
            using var db = new Curso.Data.ApplicationContext();

            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }
        // Recuperando todas a migrações existentes na aplicação
        static void TodasMigracoes()
        {
            using var db = new Curso.Data.ApplicationContext();

            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }
        // Forçando migrações em tempo de execução
        static void AplicarMigracoesEmTempoDeExecucao()
        {
            using var db = new Curso.Data.ApplicationContext();

            try
            {
                db.Database.Migrate();

                Console.WriteLine("Migração Realizada com sucesso!");
            }
            catch (System.Exception)
            {
                Console.WriteLine("Falha ao realizar a migração!");
            }
        }
        // Detectando migrações Pendentes
        static void MigracoesPendentes()
        {
            using var db = new Curso.Data.ApplicationContext();

            var migracoesPendentes = db.Database.GetPendingMigrations();

            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracao in migracoesPendentes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }
        // Proteção contra ataques de SQL Injection
        static void SqlInjection()
        {
            using var db = new Curso.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Curso.Domain.Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Curso.Domain.Departamento
                {
                    Descricao = "Departamento 02"
                });

            db.SaveChanges();
            var descricao = "Departamento 01";
            db.Database.ExecuteSqlRaw("update departamentos set descricao='Departamento Alterado' where descricao={0}", descricao);
            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
            }
        }
        //Comandos ExecuteSql
        static void ExecuteSql()
        {
            using var db = new Curso.Data.ApplicationContext();

            // Primeira Opção 
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            // Segunda Opção
            var descricao = "TESTE";
            db.Database.ExecuteSqlRaw("update departamentos set descrição={0} where id=1", descricao);

            // Terceira Opção
            db.Database.ExecuteSqlInterpolated($"update departamentos set descrição={descricao} where id=1");
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
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();

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
