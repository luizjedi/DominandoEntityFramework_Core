using System;
using System.Linq;
using EF_Functions.Scripts;
using Microsoft.EntityFrameworkCore;

namespace ef_functions
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // FuncoesDeDatas();
            // FuncaoLike();
            FuncaoDataLength();
        }

        // Função Data Length
        // Função Like
        static void FuncaoDataLength()
        {
            using (var db = new EF_Functions.Data.ApplicationContext())
            {
                var resultado = db.Funcoes
                              .AsNoTracking()
                              .Select(p =>
                                new
                                {
                                    TotalBytesCampoData = EF.Functions.DataLength(p.Data1),
                                    TotalBytes1 = EF.Functions.DataLength(p.Descricao1),
                                    TotalBytes2 = EF.Functions.DataLength(p.Descricao2),
                                    Total1 = p.Descricao1.Length,
                                    Total2 = p.Descricao2.Length
                                })
                                .FirstOrDefault();

                Console.WriteLine("Resultado:");
                Console.WriteLine(resultado);
            }
        }
        // Função Like
        static void FuncaoLike()
        {
            using (var db = new EF_Functions.Data.ApplicationContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                var dados = db.Funcoes
                              .AsNoTracking()
                              // %X consulta somente a parte final
                              // X% consulta somente a parte inicial
                              // %X% consulta todo o atributo
                              //   .Where(p => EF.Functions.Like(p.Descricao1, "%Bo%")) 
                              .Where(p => EF.Functions.Like(p.Descricao1, "B[ao]%")) // permite consulta de caracteres coringa
                              .Select(p => p.Descricao1)
                              .ToArray();

                Console.WriteLine("Resultado:");

                foreach (var descricao in dados)
                {
                    Console.WriteLine(descricao);
                }
            }
        }
        // Funções de Datas 
        static void FuncoesDeDatas()
        {
            Functions.ApagarCriarBancoDeDados();

            using (var db = new EF_Functions.Data.ApplicationContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                var dados = db.Funcoes.AsNoTracking().Select(p =>
                    new
                    {
                        Dias = EF.Functions.DateDiffDay(DateTime.Now, p.Data1), // Traz como resultado a diferença entre duas datas (Dias nesse caso).
                        Data = EF.Functions.DateFromParts(2021, 1, 2),
                        DataValida = EF.Functions.IsDate(p.Data2)
                    });

                foreach (var data in dados)
                {
                    Console.WriteLine(data);
                }
            }
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new EF_Functions.Data.ApplicationContext();
            EF_Functions.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}

