using System;
using System.Linq;
using Infraestrutura.Domain;

namespace infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // ConsultarDepartamentos();
            // DadosSensiveis();
            HabilitandoBatchSize();
        }

        // Habilitando Batch Size
        static void HabilitandoBatchSize()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 50; i++)
            {
                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento " + i
                    }
                );
            }

            db.SaveChanges();
            db.ChangeTracker.Clear();
        }
        // Dados sensíveis
        static void DadosSensiveis()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(x => x.Descricao == descricao).ToArray();
        }
        // Consultar Departamentos
        static void ConsultarDepartamentos()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToArray();
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();
            Infraestrutura.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
