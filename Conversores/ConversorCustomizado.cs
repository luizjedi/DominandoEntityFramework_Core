using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Modelo_de_Dados.Enums._Status;

namespace Modelo_de_Dados.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(
            _status => ConverterParaOhBancoDeDados(_status),
            _value => ConverterParaAplicacao(_value),
            new ConverterMappingHints(1)) 
        { } // ConverterMappingHints define a quantidade de caracteres criados no banco de dados.

        static string ConverterParaOhBancoDeDados(Status status)
        {
            return status.ToString()[0..1];
        }

        static Status ConverterParaAplicacao(string value)
        {
            var status = Enum.GetValues<Status>().FirstOrDefault(x => x.ToString()[0..1] == value);
            return status;
        }
    }
}