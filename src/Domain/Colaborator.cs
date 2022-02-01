namespace EFCore.DicasETruques.Domain
{
    public class Colaborator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region "Foreign Properties"
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        #endregion
    }
}
