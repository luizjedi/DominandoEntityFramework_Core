using System;
using System.Linq;

namespace modeloDeDados
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            Collations();
        }

        // Collations
        static void Collations()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContext();
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
