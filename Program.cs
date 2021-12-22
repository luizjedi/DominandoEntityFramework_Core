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
            FuncoesDeDatas();
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

