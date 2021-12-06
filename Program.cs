using System;
using System.Linq;

namespace StoredProcedures
{
    class Program
    {
        static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            FiltroGlobal();
        }

        // Criar ou deletar o banco de dados
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();

            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();
            Stored_Procedures.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
