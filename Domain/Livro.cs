using System.ComponentModel.DataAnnotations.Schema;

namespace UDFs.Domain
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        [Column(TypeName = "VARCHAR(15)")]
        public string Autor { get; set; }
    }
}