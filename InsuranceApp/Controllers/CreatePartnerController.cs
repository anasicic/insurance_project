using InsuranceApp.Models;
using InsuranceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InsuranceApp.Controllers
{
    [Route("[controller]")]
    public class CreatePartnerController : Controller
    {
        private readonly PartnerService _partnerService;

        public CreatePartnerController(PartnerService partnerService)
        {
            _partnerService = partnerService;
        }


        // GET metoda za prikazivanje forme
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Dohvat gradova iz baze
            var cities = await _partnerService.GetAllCitiesAsync();

            if (cities == null || !cities.Any())
            {
                ViewBag.Cities = new List<City> { new City { CityId = 1, CityName = "Zagreb" } }; // Postavite default grad
            }
            else
            {
                ViewBag.Cities = cities;
            }

            // Inicijalizacija novog partnera
            var partner = new Partner
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Address = string.Empty,
                PartnerNumber = string.Empty,
                CreateByUser = string.Empty,
                IsForeign = false,
                ExternalCode = string.Empty,
                Gender = 'M',
                CityId = 1,
                PartnerTypeId =1
            };

            return View(partner);
        }

        // POST: /CreatePartner
        [HttpPost]
        public async Task<IActionResult> Create(Partner partner)
        {
            if (partner == null || !ModelState.IsValid)
            {
                // Ako ModelState nije validan, ispisuje greške u konzolu
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                Console.WriteLine(string.Join(", ", errors));

                ModelState.AddModelError(string.Empty, "Partner data is required.");
                return View(partner); // Prikazuje formu s greškom
            }

            try
            {
                // Dodavanje partnera pomoću servisa
                await _partnerService.AddPartnerAsync(partner);

                // Provjerite je li partner ID ispravno postavljen prije nego što ga pohranite u TempData
                if (partner.PartnerId == 0)
                {
                    throw new ApplicationException("Partner ID nije ispravno postavljen.");
                }

                // Preusmjerava na neki drugi View nakon uspješnog dodavanja (npr. popis partnera)
                TempData["Success"] = partner.PartnerId;  // Spremamo ID novog partnera
                Console.WriteLine($"Partner ID {partner.PartnerId} is set in TempData.");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Dodaje grešku u ModelState i vraća formu
                ModelState.AddModelError(string.Empty, $"Error adding partner: {ex.Message}");
                return View(partner);
            }
        }
    }
}
