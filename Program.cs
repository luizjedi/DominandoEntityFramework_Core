using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Modelo_de_Dados.Enums._Status;

namespace modeloDeDados
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // Collations();
            // PropagarDados();
            // Esquema();
            // ConversorDeValor();
            ConversorCustomizado();
        }

        // Criando um conversor customizado
        static void ConversorCustomizado()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Modelo_de_Dados.Domain.Conversor
                {
                    Status = Status.Devolvido,
                });
            db.SaveChanges();
            db.ChangeTracker.Clear();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(x => x.Status == Status.Analise);

            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(x => x.Status == Status.Devolvido);
        }
        // Conversores de valor
        static void ConversorDeValor() => Esquema();
        // Esquema
        static void Esquema()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        // Propagação de dados
        static void PropagarDados()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        // Collations
        static void Collations()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContext();
            Modelo_de_Dados.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
