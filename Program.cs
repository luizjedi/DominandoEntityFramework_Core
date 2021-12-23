using System;
using System.Linq;
using Interceptacao.Domain;
using Interceptacao.Scripts;
using Microsoft.EntityFrameworkCore;

namespace interceptacao
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // TesteInterceptacao();
            TesteInterceptacaoDePersistencia();
        }

        // Teste de Interceptação de persistência
        static void TesteInterceptacaoDePersistencia()
        {
            using (var db = new Interceptacao.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Funcoes.Add(new Funcao
                {
                    Descricao1 = "Teste"
                });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        // Teste de Interceptação
        static void TesteInterceptacao()
        {
            Functions.ApagarCriarBancoDeDados();

            using (var db = new Interceptacao.Data.ApplicationContext())
            {
                var consulta = db
                            .Funcoes
                            .TagWith("Use NOLOCK")
                            .FirstOrDefault();
                Console.WriteLine($"Consulta: {consulta?.Descricao1}");
            }
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Interceptacao.Data.ApplicationContext();
            Interceptacao.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
