using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return GetEmployeeResponse(employee);
        }

        /// <summary>
        /// Создать запись о сотруднике
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(Employee employee)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _employeeRepository.CreateAsync(employee);

            return GetEmployeeResponse(item); 
        }

        /// <summary>
        /// Обновить запись о сотруднике
        /// </summary>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _employeeRepository.UpdateAsync(employee);

            return GetEmployeeResponse(item);
        }

        /// <summary>
        /// Удалить запись о сотруднике
        /// </summary>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            var item = await _employeeRepository.DeleteAsync(id);

            return Ok();
        }

        private static EmployeeResponse GetEmployeeResponse(Employee employee)
            => new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };
    }
}