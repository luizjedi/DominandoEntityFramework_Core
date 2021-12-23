using System;
using System.Linq;
using Transacoes.Domain;
using Transacoes.Scripts;

namespace transacoes
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // Functions.ApagarCriarBancoDeDados();
            ComportamentoPadrao();
        }

        // Comportamento padrão das transações
        static void ComportamentoPadrao()
        {
            CadastrarLivros();

            using (var db = new Transacoes.Data.ApplicationContext())
            {
                var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";

                db.Livros.Add(
                   new Livro
                   {
                       Titulo = "Dominando o Entity FrameWork Core",
                       Autor = "Rafael Almeida"
                   });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
        // Cadastrar Livro
        static void CadastrarLivros()
        {
            using (var db = new Transacoes.Data.ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(
                   new Livro
                   {
                       Titulo = "Introdução ao Entity FrameWork Core",
                       Autor = "Rafael"
                   });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Transacoes.Data.ApplicationContext();
            Transacoes.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
