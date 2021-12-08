using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace modeloDeDados
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // Collations();
            PropagarDados();
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
