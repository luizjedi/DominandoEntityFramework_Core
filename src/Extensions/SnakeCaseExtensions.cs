using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCore.DicasETruques.Extensions
{
    public static class SnakeCaseExtensions
    {
        public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName().ToSnakeCase();
                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                {
                    var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, null);
                    var columnName = property.GetColumnName(storeObjectIdentifier).ToSnakeCase();
                    property.SetColumnName(columnName);
                }

                foreach (var key in entity.GetKeys())
                {
                    var keyName = key.GetName().ToSnakeCase();
                    key.SetName(keyName);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    var foreignKey = key.GetConstraintName().ToSnakeCase();
                    key.SetConstraintName(foreignKey);
                }

                foreach (var index in entity.GetIndexes())
                {
                    var indexName = index.GetDatabaseName().ToSnakeCase();
                    index.SetDatabaseName(indexName);
                }
            }
        }

        // Converte o nome da variável para o nome da tabela formatado em lower case.
        private static string ToSnakeCase(this string name)
            => Regex.Replace(
                name,
                @"([a-z0-9])([A-Z])",
                "$1_$2").ToLower();
    }
}
