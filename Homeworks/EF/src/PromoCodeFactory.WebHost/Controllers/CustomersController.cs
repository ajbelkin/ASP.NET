using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Get customers list
        /// </summary>
        /// <returns>Customers list</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var response = customers.Select(x => new CustomerShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            })
                .ToList();

            return Ok(response);
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <returns>Customer</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            var response = new CustomerResponse(customer);
            return Ok(response);
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="request">Customer request</param>
        /// <returns>Creation response</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _preferenceRepository
                .GetRangeByIdsAsync(request.PreferenceIds);

            var customer = new Customer()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            })
            .Select(it => it.Preference)
            .ToList();

            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, null);
        }

        /// <summary>
        /// Edit customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <param name="request">Customer request</param>
        /// <returns>204</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            customer.Email = request.Email;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Preferences.Clear();
            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            })
            .Select(it => it.Preference)
            .ToList();

            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        /// <summary>
        /// Delete customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>204</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}