using EFCore.Testes.Data;
using EFCore.Testes.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace EFCore.Testes
{
    public class InMemoryTest
    {
        [Fact]
        public void Deve_inserir_um_departamento()
        {
            // Arrange
            var department = new Department
            {
                Description = "Tecnology",
                RegisterDate = new DateTime(2022, 02, 01)
            };

            // Setup 
            var context = CreateContext();
            context.Departments.Add(department);

            // Act 
            var insertions = context.SaveChanges();

            // Assert
            Assert.Equal(1, insertions);
        }

        [Fact]
        public void Nao_implementada_funcoes_de_datas_para_o_provider_inMemory()
        {
            // Arrange
            var department = new Department
            {
                Description = "Tecnology",
                RegisterDate = new DateTime(2022, 02, 01)
            };

            // Setup 
            var context = CreateContext();
            context.Departments.Add(department);

            // Act 
            var insertions = context.SaveChanges();

            // Assert
            Action action = () => context
                    .Departments
                    .FirstOrDefault(p => EF.Functions.DateDiffDay(DateTime.Now, p.RegisterDate) > 0);

            Assert.Throws<InvalidOperationException>(action);
        }

        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseInMemoryDatabase("InMemoryTest")
                    .Options;

            return new ApplicationContext(options);
        }
    }
}
