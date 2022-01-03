using System;
using System.Linq;
using acessandoB_D.Data;
using acessandoB_D.Domain;

namespace acessandoB_D
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                try
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    db.Pessoas.Add(new Pessoa
                    {
                        Id = 1,
                        Nome = "Luiz",
                        Telefone = "(34) 99874-3245"
                    });
                    db.SaveChanges();

                    var pessoas = db.Pessoas.ToList();

                    foreach (var pessoa in pessoas)
                    {
                        Console.WriteLine($"Nome: {pessoa.Nome}");
                    }

                    Console.WriteLine("Banco de dados criado com sucesso!");
                }
                catch (Exception error)
                {

                    throw new Exception($"Falha ao criar banco de dados!!! \n Erro: {error.Message}");
                }
            }
        }
    }
}