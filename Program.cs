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
            //ConsultaComResolucaoDeIdentidade();
            //ConsultaCustomizada();
            //ConsultaProjetadaERastreada();
            //Inserir_200_Departamentos_Com_1MB();
            ConsultaProjetada();
        }

        // Consulta Projetada 
        static void ConsultaProjetada()
        {
            using var db = new ApplicationContext();

            // var departamentos = db.Departamentos.ToArray(); // Gera uma query com aproximadamente 360 mb, pois consulta todos os campos de departamento.
            var departamentos = db.Departamentos.Select(p => p.Descricao).ToArray(); // Gera uma query com aproximadamente 60 mb, pois consulta apenas o campo selecionado(Descrição).

            // Variável que calcula quanto de memória está sendo utilizada pra realizar este processo
            var memoria = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + "MB";
            Console.WriteLine(memoria);
        }

        // Consultas Projetadas
        static void Inserir_200_Departamentos_Com_1MB()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var random = new Random();

                db.Departamentos.AddRange(Enumerable.Range(1, 200).Select(p => new Departamento
                {
                    Descricao = $"Departamento Teste",
                    Imagem = getBytes()
                }));
                db.SaveChanges();

                byte[] getBytes()
                {
                    var buffer = new byte[1024 * 1024];
                    random.NextBytes(buffer);

                    return buffer;
                }
            }
        }

        // Consulta Projetada e Rastreada
        static void ConsultaProjetadaERastreada()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos
                .Include(p => p.Funcionarios)
                .Select(p => new
                {
                    Departamento = p,
                    TotalFuncionarios = p.Funcionarios.Count()
                })
                .ToList();

            departamentos[0].Departamento.Descricao = "Teste de Atualização do Departamento";
            db.SaveChanges();
        }

        // Consulta Customizada
        static void ConsultaCustomizada()
        {
            using var db = new ApplicationContext();

            // Desabilitando o rastreamento de forma local
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;

            var funcionarios = db.Funcionarios
                //.AsNoTrackingWithIdentityResolution()
                .Include(p => p.Departamento)
                .ToList();
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
