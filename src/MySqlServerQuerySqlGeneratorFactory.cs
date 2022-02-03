using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EFCore
{
    class MySqlServerQuerySqlGeneratorFactory : SqlServerQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;

        public MySqlServerQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
        {
            this._dependencies = dependencies;
        }

        public override QuerySqlGenerator Create()
        {
            return new MySqlServerQuerySqlGenerator(_dependencies);
        }
    }
}
