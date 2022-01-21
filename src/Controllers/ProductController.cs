using EFCore.MultiTenant.Data;
using EFCore.MultiTenant.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.MultiTenant.Controllers
{
    [ApiController]
    [Route("{tenant}/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get([FromServices] ApplicationContext db)
        {
            var products = db.Products.ToArray();

            return products;
        }
    }
}
