using Microsoft.EntityFrameworkCore;
using performance.Data;
using performance.Domain;
using System;
using System.Linq;

namespace performance
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            //ConsultaRastreada();
            //ConsultaNaoRastreada();
            ConsultaComResolucaoDeIdentidade();
        }

        // Consulta NÃO Rastreada com resolução de Identidade
        static void ConsultaComResolucaoDeIdentidade()
        {
            using var db = new ApplicationContext();

            var funcionarios = db.Funcionarios.AsNoTrackingWithIdentityResolution().Include(p => p.Departamento).ToList();
        }

        // Consulta NÃO Rastreada
        static void ConsultaNaoRastreada()
        {
            using var db = new ApplicationContext();

            var funcionarios = db.Funcionarios.AsNoTracking().Include(p => p.Departamento).ToList();
        }

        // Consulta Rastreada
        static void ConsultaRastreada()
        {
            using var db = new ApplicationContext();

            var funcionarios = db.Funcionarios.Include(p => p.Departamento).ToList();
        }

        // Setup de criação do banco
        static void Setup()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Departamentos.Add(new Departamento
                {
                    Descricao = "Departamento Jurídico",
                    Ativo = true,
                    Funcionarios = Enumerable.Range(1, 100).Select(p => new Funcionario
                    {
                        CPF = p.ToString().PadLeft(11, '0'),
                        Nome = $"Funcionário {p}",
                        RG = p.ToString()
                    }).ToList()
                });
                db.SaveChanges();
            }
        }
    }
}
