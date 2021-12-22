using System;
using EF_Functions.Domain;

namespace EF_Functions.Scripts
{
    public class Functions
    {
        public static void ApagarCriarBancoDeDados()
        {
            using (var db = new EF_Functions.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Funcoes.AddRange(
                    new Funcao
                    {
                        Data1 = DateTime.Now.AddDays(2),
                        Data2 = "2021-01-01",
                        Descricao1 = "Bala 1",
                        Descricao2 = "Bala 1"
                    },
                    new Funcao
                    {
                        Data1 = DateTime.Now.AddDays(1),
                        Data2 = "XX21-01-01",
                        Descricao1 = "Bola 2",
                        Descricao2 = "Bola 2"
                    },
                    new Funcao
                    {
                        Data1 = DateTime.Now.AddDays(1),
                        Data2 = "XX21-01-01",
                        Descricao1 = "Tela",
                        Descricao2 = "Tela"
                    }
                );

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }
}