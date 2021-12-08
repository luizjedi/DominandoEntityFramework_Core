using System.Net;
using static Modelo_de_Dados.Enums._Status;
using static Modelo_de_Dados.Enums._Versao;

namespace Modelo_de_Dados.Domain
{
    public class Conversor
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public Versao Versao { get; set; }
        public IPAddress EnderecoIP { get; set; }
        public Status Status { get; set; }
    }
}