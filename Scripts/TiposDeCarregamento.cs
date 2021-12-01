using System.Linq;

namespace Curso.Scripts
{
    public class TiposDeCarregamento
    {
        public static void SetupTiposDeCarregamento(Curso.Data.ApplicationContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Curso.Domain.Departamento
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new System.Collections.Generic.List<Domain.Funcionario>{
                            new Curso.Domain.Funcionario{
                                Nome="Rafael Almeida",
                                CPF="999.999.999-11",
                                RG="MG-19-786.327"
                            }
                        }
                    },
                    new Curso.Domain.Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Domain.Funcionario>{
                            new Curso.Domain.Funcionario{
                                Nome="Bruno Brito",
                                CPF="888.888.888-11",
                                RG="MG-18-786.322"
                            },
                            new Curso.Domain.Funcionario{
                                Nome="Eduardo Pires",
                                CPF="777.777.777-11",
                                RG="MG-15-789.342"
                            }
                        }
                    }
                );

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }
}