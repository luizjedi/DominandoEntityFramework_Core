using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Modelo_de_Dados.Domain;
using static Modelo_de_Dados.Enums._Status;

namespace modeloDeDados
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // Collations();
            // PropagarDados();
            // Esquema();
            // ConversorDeValor();
            // ConversorCustomizado();
            // PropriedadesDeSombra();
            //TrabalhandoComPropriedadesDeSombra();
            // TiposDePropriedades();
            // Relacionamento1Para1();
            // Relacionamento1ParaMuitos();
            RelacionamentoMuitosParaMuitos();
        }

        // Relacionamento muitos para muitos
        static void RelacionamentoMuitosParaMuitos()
        {
            using (var db = new Modelo_de_Dados.Data.ApplicationContextIndice())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var ator1 = new Ator { Nome = "Keanu Reeves" };
                var ator2 = new Ator { Nome = "Tom Cruise" };
                var ator3 = new Ator { Nome = "Johnny Depp" };

                var filme1 = new Filme { Descricao = "Matrix 3" };
                var filme3 = new Filme { Descricao = "Missão Impossível" };
                var filme2 = new Filme { Descricao = "Matrix 4" };

                ator1.Filmes.Add(filme1);
                ator1.Filmes.Add(filme2);

                ator2.Filmes.Add(filme1);

                filme3.Atores.Add(ator1);
                filme3.Atores.Add(ator2);
                filme3.Atores.Add(ator3);

                db.AddRange(ator1, ator2, filme3);

                db.SaveChanges();
                db.ChangeTracker.Clear();

                foreach (var ator in db.Atores)
                {
                    Console.WriteLine($"Ator: {ator.Nome}");

                    foreach (var filme in ator.Filmes)
                    {
                        Console.WriteLine($"\t Filme: {filme.Descricao}");
                    }
                }
            }
        }
        // Relacionamento 1 para muitos
        static void Relacionamento1ParaMuitos()
        {
            using (var db = new Modelo_de_Dados.Data.ApplicationContextIndice())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var estado = new Estado
                {
                    Nome = "Sergipe",
                    Governador = new Governador { Nome = "Luiz Felipe de Oliveira" }
                };

                estado.Cidades.Add(new Cidade { Nome = "Itabaiana" });

                db.Estados.Add(estado);
                db.SaveChanges();
            }

            using (var db = new Modelo_de_Dados.Data.ApplicationContextIndice())
            {
                var estados = db.Estados.ToList();

                estados[0].Cidades.Add(new Cidade { Nome = "Aracajú" });

                db.SaveChanges();
                db.ChangeTracker.Clear();

                foreach (var est in db.Estados.Include(p => p.Cidades).AsNoTracking())
                {
                    Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");

                    foreach (var cid in est.Cidades)
                    {
                        Console.WriteLine($"\t Cidade: {cid.Nome}");
                    }
                }
            }
        }
        // Relacionamento 1 para 1
        static void Relacionamento1Para1()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Sergipe",
                Governador = new Governador { Nome = "Luiz Felipe de Oliveira" }
            };

            db.Estados.Add(estado);
            db.SaveChanges();
            db.ChangeTracker.Clear();

            var estados = db.Estados.AsNoTracking().ToList();

            estados.ForEach(est =>
            {
                Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
            });
        }
        // Tipos de Propriedades
        static void TiposDePropriedades()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var cliente = new Cliente
            {
                Nome = "Hanna de Oliveira",
                Telefone = "(34) 99874-7894",
                Endereco = new Endereco { Bairro = "Centro", Cidade = "São Paulo" }
            };

            db.Clientes.Add(cliente);
            db.SaveChanges();
            db.ChangeTracker.Clear();

            var clientes = db.Clientes.AsNoTracking().ToList();

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };

            clientes.ForEach(cli =>
            {
                var json = System.Text.Json.JsonSerializer.Serialize(cli, options);
                Console.WriteLine(json);
            });
        }
        // Utilizando Shadow Properties
        static void TrabalhandoComPropriedadesDeSombra()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();

            // var departamento = new Departamento
            // {
            //     Descricao = "Departamento Propriedade de Sombra"
            // };

            // db.Departamentos.Add(departamento);

            // db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            // db.SaveChanges();
            // db.ChangeTracker.Clear();

            var departamentos = db.Departamentos.Where(x => EF.Property<DateTime>(x, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }
        // Utilizando Shadow Properties
        static void PropriedadesDeSombra()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        // Criando um conversor customizado
        static void ConversorCustomizado()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Modelo_de_Dados.Domain.Conversor
                {
                    Status = Status.Devolvido,
                });
            db.SaveChanges();
            db.ChangeTracker.Clear();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(x => x.Status == Status.Analise);

            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(x => x.Status == Status.Devolvido);
        }
        // Conversores de valor
        static void ConversorDeValor() => Esquema();
        // Esquema
        static void Esquema()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        // Propagação de dados
        static void PropagarDados()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }
        // Collations
        static void Collations()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContextIndice();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Modelo_de_Dados.Data.ApplicationContext();
            Modelo_de_Dados.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
