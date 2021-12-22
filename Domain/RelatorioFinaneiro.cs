using System;
using Microsoft.EntityFrameworkCore;

namespace Atributos.Domain
{
    [Keyless]
    public class RelatorioFinaneiro
    {
        public string Descricao { get; set; }
        public decimal Total { get; set; }
        public DateTime Data { get; set; }
    }
}