using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Promocodes
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Preference> _preferencesRepository;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository, IRepository<Customer> customersRepository, IRepository<Preference> preferencesRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _customersRepository = customersRepository;
            _preferencesRepository = preferencesRepository;
        }

        /// <summary>
        /// Get promocodes list
        /// </summary>
        /// <returns>Promocodes list</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var preferences = await _promoCodesRepository.GetAllAsync();

            var response = preferences.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = $"{x.BeginDate:yyyy-MM-dd}",
                EndDate = $"{x.EndDate:yyyy-MM-dd}",
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var customers = await _customersRepository.GetAllAsync();
            var preferences = await _preferencesRepository.GetAllAsync();

            var preference = preferences.FirstOrDefault(a => a.Name == request.Preference);
            var promocode = new PromoCode { Id = Guid.NewGuid(), Code = request.PromoCode, Preference = preference };
            var updatedCustomers = customers.Where(it => it.Preferences.Contains(preference));
            foreach (var customer in updatedCustomers)
            {
                customer.PromoCodes.Add(promocode);
            }

            return Ok(updatedCustomers);
        }
    }
}