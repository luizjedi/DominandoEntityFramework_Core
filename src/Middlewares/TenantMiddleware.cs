using EFCore.MultiTenant.Extensions;
using EFCore.MultiTenant.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace EFCore.MultiTenant.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public  async Task InvokeAsync(HttpContext httpContext)
        {
            var tenant = httpContext.RequestServices.GetRequiredService<TenantData>();

            tenant.TenantId = httpContext.GetTenantId();

            await _next(httpContext);
        }
    }
}
