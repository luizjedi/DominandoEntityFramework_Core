using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UDFs.Domain;

namespace udfs
{
    class Program
    {
        static void Main(string[] args)
        {
            // FuncaoLEFT();
            // FuncaoDefinidaPeloUsuario();
            DateDIFF();
        }

        // Customização de uma função complexa 
        static void DateDIFF()
        {
            CadastrarLivros();

            using (var db = new UDFs.Data.ApplicationContext())
            {
                // 1ª Forma:
                // var resultado = db
                //        .Livros
                //        // Retorna a diferença de dias entre o primeiro(p.CadastradoEm) e o segundo(DateTime.Now) parâmetro.
                //        .Select(p => EF.Functions.DateDiffDay(p.CadastradoEm, DateTime.Now));

                // 2ª Forma:
                var resultado = db
                       .Livros
                       .Select(p => UDFs.Functions.MyFunctions.DateDIFF("DAY", p.CadastradoEm, DateTime.Now));

                foreach (var diff in resultado)
                {
                    Console.WriteLine(diff);
                }
            }
        }
        // Função definida pelo usuário
        static void FuncaoDefinidaPeloUsuario()
        {
            CadastrarLivros();

            using (var db = new UDFs.Data.ApplicationContext())
            {
                db.Database.ExecuteSqlRaw(@"
                 CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
                 RETURNS VARCHAR(100)
                 BEGIN
                    RETURN UPPER(@dados)
                END");

                var resultado = db.Livros.Select(p => UDFs.Functions.MyFunctions.LetrasMaiusculas(p.Titulo));
                foreach (var maiusculas in resultado)
                {
                    Console.WriteLine(maiusculas);
                }
            }
        }
        // Função LEFT via data annotation(ApplicationContext)
        static void FuncaoLEFT()
        {
            CadastrarLivros();

            using var db = new UDFs.Data.ApplicationContext();

            var resultado = db.Livros.Select(p => UDFs.Functions.MyFunctions.Left(p.Titulo, 10));
            foreach (var part in resultado)
            {
                Console.WriteLine(part);
            }
        }
        // Cadastrar Livros
        static void CadastrarLivros()
        {
            using (var db = new UDFs.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(new Livro
                {
                    Titulo = "Introdução ao Entity Framework Core",
                    Autor = "Luiz",
                    CadastradoEm = DateTime.Now.AddDays(-1)
                });

                db.SaveChanges();
            }
        }
    }
}
