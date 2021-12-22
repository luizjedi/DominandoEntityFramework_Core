using System;
using System.Linq;
using Atributos.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAnnotations
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // AtributoTable();
            AtributoDatabaseGenerated();
        }

        // Atributo DatabaseGenerated
        static void AtributoDatabaseGenerated()
        {
            using (var db = new Atributos.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                db.Atributos.Add(new Atributo
                {
                    Descricao = "Exemplo",
                    Observacao = "Observation"
                });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        // Atributo Table
        static void AtributoTable()
        {
            using (var db = new Atributos.Data.ApplicationContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);
            }
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Atributos.Data.ApplicationContext();
            Atributos.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
