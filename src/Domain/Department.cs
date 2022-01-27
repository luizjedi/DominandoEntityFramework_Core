using System.Collections.Generic;

namespace EFCore.UoWRepository.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public List<Colaborator> Colaborators { get; set; }
    }
}
