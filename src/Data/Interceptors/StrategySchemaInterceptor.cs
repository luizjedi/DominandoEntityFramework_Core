using EFCore.MultiTenant.Provider;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.MultiTenant.Data.Interceptors
{
    public class StrategySchemaInterceptor : DbCommandInterceptor
    {
        private readonly TenantData _tenantData;

        public StrategySchemaInterceptor(TenantData tenantData)
        {
            this._tenantData = tenantData;
        }

        private void ReplaceSchema(DbCommand command)
        {
            command.CommandText = command.CommandText
                .Replace("FROM ", $" FROM [{_tenantData.TenantId}].")
                .Replace("JOIN ", $" JOIN [{_tenantData.TenantId}].");
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            ReplaceSchema(command);

            return base.ReaderExecuting(command, eventData, result);
        }
    }
}
