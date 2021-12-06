using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            // FiltroGlobal();
            // IgnoreFiltroGlobal();
            // ConsultaProjetada();
            // ConsultaParametrizada();
            // ConsultaInterpolada();
            // ConsultaComTAG();
            // EntendendoConsulta1NN1();
            DivisaoDeConsulta();
        }
        // Criar ou deletar o banco de dados
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Consultas.Data.ApplicationContext();

            db.Database.EnsureCreated();
            // db.Database.EnsureDeleted();
        }

        // Criando uma consulta Interpolada
        static void DivisaoDeConsulta()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos
                    .Include(x => x.Funcionarios)
                    .Where(p => p.Id < 3)
                    // .AsSplitQuery() Faz a Divisão das consultas 
                    .AsSingleQuery() // Ignora a configuração global de divisão de consultas caso essas estejam ativas
                    .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }
            Console.WriteLine("----------------------------");
        }
        // Entendendo a dirença entre consultas 1xN e Nx1
        static void EntendendoConsulta1NN1()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            // Consulta de 1 x N

            // var departamentos = db.Departamentos
            //         .Include(x => x.Funcionarios)
            //         .ToList();

            // foreach (var departamento in departamentos)
            // {
            //     Console.WriteLine("----------------------------");
            //     Console.WriteLine($"Descrição: {departamento.Descricao}");

            //     foreach (var funcionario in departamento.Funcionarios)
            //     {
            //         Console.WriteLine($"\tNome: {funcionario.Nome}");
            //     }
            // }
            // Console.WriteLine("----------------------------");

            // Consulta de N x 1

            var funcionarios = db.Funcionarios
                    .Include(x => x.Departamento)
                    .ToList();

            foreach (var funcionario in funcionarios)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine($"Nome: {funcionario.Nome} -- Descrição: {funcionario.Departamento.Descricao}");
            }
            Console.WriteLine("------------------------------------------------------");
        }
        // Criando uma consulta com TAGS
        static void ConsultaComTAG()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos
                    // Ao adicionar "@" se torna possível realizar comentários multilines
                    .TagWith(@"Estou enviando um comentário para o server  
                    Segundo comentário
                    Terceiro comentário
                    ...")
                    .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
            Console.WriteLine("----------------------------");
        }
        // Criando uma consulta Interpolada
        static void ConsultaInterpolada()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var id = 1;
            var departamentos = db.Departamentos
                    .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}")
                    .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
            Console.WriteLine("----------------------------");
        }
        // Criando uma consulta parametrizada
        static void ConsultaParametrizada()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var id = new SqlParameter
            {
                Value = 1,
                SqlDbType = System.Data.SqlDbType.Int
            };
            var departamentos = db.Departamentos
                    .FromSqlRaw("SELECT * FROM Departamentos WHERE Id>{0}", id)
                    .Where(x => !x.Excluido) // Cria uma subconsulta 
                    .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
            Console.WriteLine("----------------------------");
        }
        // Criando uma consulta projetada
        static void ConsultaProjetada()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos
                    .Where(x => x.Id > 0)
                    .Select(x => new { x.Descricao, Funcionarios = x.Funcionarios.Select(f => f.Nome) }) // Seleciona apenas os campos desejados do objeto
                    .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                Console.WriteLine("----------------------------");
                Console.WriteLine("Funcionários: \r\n");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
                Console.WriteLine("____________________________");
            }
        }
        // Ignorando filtro Global
        static void IgnoreFiltroGlobal()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Consultas.Data.ApplicationContext();
            Consultas.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
