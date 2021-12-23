using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Interceptacao.Interceptadores
{
    public class InterceptadorDeConexao : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result)
        {
            Console.WriteLine("Entrei no método ConnectionOpening");

            var connectionString = ((SqlConnection)connection).ConnectionString;

            Console.WriteLine(connectionString);

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                ApplicationName = "Atributos"
            };

            connection.ConnectionString = connectionStringBuilder.ToString();

            Console.WriteLine(connectionStringBuilder.ToString());

            return result;
        }
    }
}