﻿namespace EFCore.UoWRepository.Domain
{
    public class Colaborator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
