using System;
using System.Linq;

namespace ef_functions
{
    class Program
    {
        static void Main(string[] args)
        {
            FiltroGlobal();
        }

        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new ef_functions.Data.ApplicationContext();
            ef_functions.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}

