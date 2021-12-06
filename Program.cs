using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace StoredProcedures
{
    class Program
    {
        static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            // FiltroGlobal();
            // CriarStoredProcedure();
            // InserirDadosProcedure();
            // CriarStoredProcedureDeConsulta();
            ConsultaViaProcedure();
        }

        // Criar ou deletar o banco de dados
        static void EnsureCreatedAndDeleted()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();

            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }

        // Criando uma procedure de consultas
        static void ConsultaViaProcedure()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();

            var dep = new SqlParameter("@dep", "Departamento"); // Permite nomear e declarar os parâmetros em uma variável, o que aumenta nível de encapsulamento

            var departamentos = db.Departamentos
                // .FromSqlRaw("EXECUTE GetDepartamentos @dep", dep) // primeira forma 
                .FromSqlInterpolated($"EXECUTE GetDepartamentos {dep}") // segunda forma 
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }
        // Criando uma procedure de consultas
        static void CriarStoredProcedureDeConsulta()
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE GetDepartamentos
                    @Descricao VARCHAR(50)
                AS
                BEGIN
                    SELECT * FROM Departamentos WHERE Descricao LIKE @Descricao + '%' 
                END
                "; // o operador '%' indica que a consulta será executada pelas iniciais

            using var db = new Stored_Procedures.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }
        // Executando inserção via procedure
        static void InserirDadosProcedure()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true);
        }
        // Criando uma procedure de inserção
        static void CriarStoredProcedure()
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE CriarDepartamento 
                @Descricao VARCHAR(50),
                @Ativo bit
                AS
                BEGIN
                    INSERT INTO
                        Departamentos(Descricao, Ativo, Excluido)
                    VALUES (@Descricao, @Ativo, 0)
                END
                ";

            using var db = new Stored_Procedures.Data.ApplicationContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }
        // Filtro Global
        static void FiltroGlobal()
        {
            using var db = new Stored_Procedures.Data.ApplicationContext();
            Stored_Procedures.Scripts.Initial.Setup(db);

            var departamentos = db.Departamentos.Where(x => x.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }
    }
}
