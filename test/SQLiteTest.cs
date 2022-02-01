using EFCore.Testes.Data;
using EFCore.Testes.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace EFCore.Testes
{
    public class SQLiteTest
    {
        [Theory]
        [InlineData("Tecnology")]
        [InlineData("Financial")]
        [InlineData("Personal Deparment")]
        public void Deve_inserir_e_consultar_um_departamento(string description)
        {
            // Arrange
            var department = new Department
            {
                Description = description,
                RegisterDate = new DateTime(2022, 02, 01)
            };

            // Setup 
            var context = CreateContext();
            context.Database.EnsureCreated();
            context.Departments.Add(department);

            // Act 
            var insertions = context.SaveChanges();
            department = context.Departments.FirstOrDefault(c => c.Description == description);

            // Assert
            Assert.Equal(1, insertions);
            Assert.Equal(description, department.Description);
        }

        private ApplicationContext CreateContext()
        {
            //const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Testes;Integrated Security=true;pooling=true";

            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite(connection)
                    .Options;

            return new ApplicationContext(options);
        }
    }
}
