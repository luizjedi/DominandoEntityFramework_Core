using System;
using System.Linq;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Transacoes.Domain;

namespace transacoes
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // Functions.ApagarCriarBancoDeDados();
            // ComportamentoPadrao();
            // GerenciandoTransacaoManualmente();
            // ReverterTransacao();
            // SalvarPontoTransacao();
            TransactionScope();
        }

        // TrasactionScope
        static void TransactionScope()
        {
            CadastrarLivros();

            var trasOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, trasOptions))
            {
                ConsultarAtualizar();
                CadastrarLivroEnterprise();
                CadastrarLivroDominandoEFCore();

                scope.Complete(); // Caso ocorra alguma exceção ele faz o rollback automaticamete.
            }
        }

        static void CadastrarLivroDominandoEFCore()
        {
            using (var db = new Transacoes.Data.ApplicationContext())
            {
                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Dominando o Entity Framework Core",
                        Autor = "Luiz Felipe"
                    });
                db.SaveChanges();
            }
        }

        static void CadastrarLivroEnterprise()
        {
            using (var db = new Transacoes.Data.ApplicationContext())
            {
                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "ASP.NET Core Enterprise Applications",
                        Autor = "Eduardo Pires"
                    });
                db.SaveChanges();
            }
        }

        static void ConsultarAtualizar()
        {
            using (var db = new Transacoes.Data.ApplicationContext())
            {
                var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";
                db.SaveChanges();
            }
        }
        // Salvando o ponto de uma transação.
        static void SalvarPontoTransacao()
        {
            CadastrarLivros();

            using (var db = new Transacoes.Data.ApplicationContext())
            {
                var transacao = db.Database.BeginTransaction();

                try
                {
                    var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                    livro.Autor = "Luiz Felipe";
                    db.SaveChanges();

                    // Se houver algum rollback durante a transação ele volta até este ponto em vez de descartar todas alterações.
                    transacao.CreateSavepoint("desfazer_apenas_insercao");

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "ASP.NET Core Enterprise Applications",
                            Autor = "Eduardo Pires"
                        });
                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando o Entity Framework Core",
                            Autor = "Rafael Almeida".PadLeft(16, '@')
                        });
                    db.SaveChanges();

                    transacao.Commit();
                }
                catch (DbUpdateException error)
                {
                    Console.WriteLine($"Erro: {error.Message}");
                    transacao.RollbackToSavepoint("desfazer_apenas_insercao"); // Realiza rollback personalizado até o ponto de salvamento criado.

                    if (error.Entries.Count(p => p.State == EntityState.Added) == error.Entries.Count)
                    {
                        transacao.Commit();
                    }
                }
            }
        }
        // Utilizando Rollback para reverter uma transação.
        static void ReverterTransacao()
        {
            CadastrarLivros();

            using (var db = new Transacoes.Data.ApplicationContext())
            {
                var transacao = db.Database.BeginTransaction(); // Cria uma instância de BeginTransaction.

                try
                {
                    var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                    livro.Autor = "Rafael Almeida";
                    db.SaveChanges();

                    db.Livros.Add(
                       new Livro
                       {
                           Titulo = "Dominando o Entity FrameWork Core",
                           Autor = "Rafael Almeida".PadLeft(16, '#')
                       });

                    db.SaveChanges();

                    transacao.Commit();
                }
                catch (Exception error)
                {
                    transacao.Rollback();
                    Console.WriteLine($"Não foi possível realizar essa transação! \n {error.Message}");
                }
            }
        }
        //Gerenciando Transação Manualmente
        static void GerenciandoTransacaoManualmente()
        {
            CadastrarLivros();

            using (var db = new Transacoes.Data.ApplicationContext())
            {
                var transacao = db.Database.BeginTransaction(); // Cria uma instância de BeginTransaction.

                var livro = db.Livros.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";
                db.SaveChanges();

                Console.ReadKey();

                db.Livros.Add(
                   new Livro
                   {
                       Titulo = "Dominando o Entity FrameWork Core",
                       Autor = "Rafael Almeida"
                   });

                db.SaveChanges();

                transacao.Commit();
            }
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
            }
        }

        // Filtro Global
        // static void FiltroGlobal()
        // {
        //     using var db = new Transacoes.Data.ApplicationContext();
        //     Transacoes.Scripts.Initial.Setup(db);

        //     var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

        //     foreach (var departamento in departamentos)
        //     {
        //         Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
        //     }
        // }
    }
}
