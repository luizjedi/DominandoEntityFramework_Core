using System;
using System.Linq;
using Infraestrutura.Domain;
using Microsoft.EntityFrameworkCore;

namespace infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // ConsultarDepartamentos();
            // DadosSensiveis();
            // HabilitandoBatchSize();
            // TempoComandoGeral();
            // TempoComandoPara1Fluxo();
            ExecutarEstrategiaResiiencia();
        }

        // Criando uma estratégia de resiliência
        static void ExecutarEstrategiaResiiencia()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Departamento { Descricao = "Departamento Transação" });
                db.SaveChanges();
                db.ChangeTracker.Clear();

                transaction.Commit();
            });
        }
        // Configurando o Timeout do comando para um fluxo
        static void TempoComandoPara1Fluxo()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            db.Database.SetCommandTimeout(10);

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
        }
        // Configurando o Timeout do comando global
        static void TempoComandoGeral()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
        }

        // Habilitando Batch Size
        static void HabilitandoBatchSize()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (var i = 0; i < 50; i++)
            {
                db.Departamentos.Add(
                    new Departamento
                    {
                        Descricao = "Departamento " + i
                    }
                );
            }

            db.SaveChanges();
            db.ChangeTracker.Clear();
        }
        // Dados sensíveis
        static void DadosSensiveis()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(x => x.Descricao == descricao).ToArray();
        }
        // Consultar Departamentos
        static void ConsultarDepartamentos()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToArray();
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Infraestrutura.Data.ApplicationContext();
            Infraestrutura.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
