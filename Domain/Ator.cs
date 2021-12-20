using System.Collections.Generic;

namespace Modelo_de_Dados.Domain
{
    public class Ator
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Filme> Filmes { get; } = new List<Filme>();
    }
}