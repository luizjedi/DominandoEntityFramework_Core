using EFCore.UoWRepository.Data._UnitOfWork;
using EFCore.UoWRepository.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EFCore.UoWRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        //private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(ILogger<DepartmentController> logger, /*IDepartmentRepository repository, */IUnitOfWork unitOfWork)
        {
            //this._repository = repository;
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }


        #region "GET"
        // Primeira Opção
        //[HttpGet("{id}")] // departamento/{id}
        //public async Task<IActionResult> GetByIdAsync(int id, [FromServices] IDepartmentRepository repository)
        //{
        //    var department = await repository.GetbyIdAsunc(id);

        //    return Ok(department);
        //}

        // Segunda Opção ****** Melhor implementação
        [HttpGet("{id}")] // departamento/{id}
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            //var department = await this._repository.GetbyIdAsunc(id);
            var department = await this._unitOfWork.Department.GetByIdAsync(id);

            return Ok(department);
        }
        
        
        [HttpGet]  // departamento/?descrição=teste
        public async Task<IActionResult> QueryDepartmentAsync([FromQuery] string descricao)
        {
            var departments = await this._unitOfWork.Department.GetDataAsync(
                d => d.Description.Contains(descricao),
                i => i.Include(x => x.Colaborators),
                take: 2
                );

            return Ok(departments);
        }
        #endregion

        #region "POST"
        [HttpPost]
        public IActionResult CreateDepartment(Department department)
        {
            this._unitOfWork.Department.Add(department);
            this._unitOfWork.Coommit();

            return Ok(department);
        }
        #endregion

        #region "DELETE"
        [HttpDelete("{id}")]  // departamento/{id}
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await this._unitOfWork.Department.GetByIdAsync(id);

            this._unitOfWork.Department.Remove(department);
            this._unitOfWork.Coommit();

            return Ok(department);
        }
        #endregion
    }
}
