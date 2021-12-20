using System.Collections.Generic;

namespace Modelo_de_Dados.Domain
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Governador Governador { get; set; }
        public ICollection<Cidade> Cidades { get; } = new List<Cidade>();
    }
}