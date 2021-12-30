using Microsoft.EntityFrameworkCore;
using migrations.Data;
using System;

namespace migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                // Aplicando migrações no banco de dados em tempo de execução.
                //db.Database.Migrate();

                //Listando aplicações pendentes em tempo de execução.
                var migracoes = db.Database.GetPendingMigrations();

                foreach (var migracao in migracoes)
                {
                    Console.WriteLine(migracao);
                }
            }
        }


    }
}
