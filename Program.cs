using System;

namespace StoredProcedures
{
    class Program
    {
        static void Main(string[] args)
        {
            EnsureCreatedAndDeleted();
        }

        // Criar ou deletar o banco de dados
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();

            db.Database.EnsureCreated();
            // db.Database.EnsureDeleted();
        }
    }
}
