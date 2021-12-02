using System;
using System.Linq;
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
            ConsultaProjetada();
        }

        // Criar ou deletar o banco de dados
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Consultas.Data.ApplicationContext();

            db.Database.EnsureCreated();
            // db.Database.EnsureDeleted();
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
