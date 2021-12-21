using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAnnotations
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            AtributoTable();
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
