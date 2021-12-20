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
            TrabalhandoComPropriedadesDeSombra();
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
