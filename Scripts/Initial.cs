namespace Atributos.Scripts
{
    public class Initial
    {
        public static void Setup(Atributos.Data.ApplicationContext db)
        {
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                   new Atributos.Domain.Departamento
                   {
                       Descricao = "Departamento 01",
                       Funcionarios = new System.Collections.Generic.List<Domain.Funcionario>{
                            new Atributos.Domain.Funcionario{
                                Nome="Rafael Almeida",
                                CPF="999.999.999-11",
                                RG="MG-19-786.327"
                            }
                        },
                       Excluido = true
                   },
                    new Atributos.Domain.Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new System.Collections.Generic.List<Domain.Funcionario>{
                            new Atributos.Domain.Funcionario{
                                Nome="Bruno Brito",
                                CPF="888.888.888-11",
                                RG="MG-18-786.322"
                            },
                            new Atributos.Domain.Funcionario{
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