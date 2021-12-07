using System;
using System.Linq;

namespace infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            // FiltroGlobal();
            // ConsultarDepartamentos();
            DadosSensiveis();
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
