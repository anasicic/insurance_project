using InsuranceApp.Models;
using InsuranceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InsuranceApp.Controllers
{
    [Route("[controller]")] // Defines the route for this controller (CreatePartner)
    public class CreatePartnerController : Controller
    {
        private readonly PartnerService _partnerService;    // Declare the partner service that will be used to interact with data

        // Constructor: inject the PartnerService into the controller
        public CreatePartnerController(PartnerService partnerService)
        {
            _partnerService = partnerService;   // Assign the service to a private field
        }


        // GET method for displaying the form to create a new partner
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Retrieve cities from the database using PartnerService
            var cities = await _partnerService.GetAllCitiesAsync();

            // If no cities are found or the list is empty, set a default city (Zagreb)
            if (cities == null || !cities.Any())
            {
                ViewBag.Cities = new List<City> { new City { CityId = 1, CityName = "Zagreb" } }; // Set default city
            }
            else
            {
                ViewBag.Cities = cities;
            }

            // Initialize a new partner with default values (empty strings, false for boolean, etc.)
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
                CityId = 0,
                PartnerTypeId =0
            };

            // Return the View with the partner model (used for displaying the form)
            return View(partner);
        }

        // POST: /CreatePartner
        [HttpPost]
        public async Task<IActionResult> Create(Partner partner)
        {
            //If the partner object is null or ModelState is not valid (validation failed), show errors
            if (partner == null || !ModelState.IsValid)
            {
                
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                Console.WriteLine(string.Join(", ", errors));

                ModelState.AddModelError(string.Empty, "Partner data is required.");
                return View(partner); // Returns form with error
            }

            try
            {
                // Call the PartnerService to add the partner to the database
                await _partnerService.AddPartnerAsync(partner);

                // Check if the PartnerId is correctly set after the partner is added
                if (partner.PartnerId == 0)
                {
                    throw new ApplicationException("Partner ID nije ispravno postavljen.");
                }

                // Store the newly created PartnerId in TempData to use it in another view 
                TempData["Success"] = partner.PartnerId;  
                Console.WriteLine($"Partner ID {partner.PartnerId} is set in TempData.");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // If an error occurs, add it to the ModelState and return the form
                ModelState.AddModelError(string.Empty, $"Error adding partner: {ex.Message}");
                return View(partner);
            }
        }
    }
}
