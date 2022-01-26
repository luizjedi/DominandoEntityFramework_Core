using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.MultiTenant.Migrations
{
    public partial class Estrategia03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Products",
                schema: "dbo",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Persons",
                schema: "dbo",
                newName: "Persons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Persons",
                newSchema: "dbo");
        }
    }
}
