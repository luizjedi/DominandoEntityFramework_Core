using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UDFs.Functions
{
    public class MyFunctions
    {
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        public static string LetrasMaiusculas(string dados)
        {
            throw new NotImplementedException();
        }

        public static int DateDIFF(string identificador, DateTime dataInicial, DateTime dataFinal)
        {
            throw new NotImplementedException();
        }

        public static void FunctionsRegister(ModelBuilder modelBuilder)
        {
            // Leitura de todos métodos que possuem o atributo especificado.
            var functions = typeof(MyFunctions).GetMethods().Where(p => Attribute.IsDefined(p, typeof(DbFunctionAttribute)));

            foreach (var func in functions)
            {
                modelBuilder.HasDbFunction(func);
            }
        }
    }
}