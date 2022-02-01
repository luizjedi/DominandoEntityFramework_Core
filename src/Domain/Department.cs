using System.Collections.Generic;

namespace EFCore.DicasETruques.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Description { get; set; }

        #region "Foreign Properties"
        public List<Colaborator> Colaborators { get; set; }
        #endregion
    }
}
