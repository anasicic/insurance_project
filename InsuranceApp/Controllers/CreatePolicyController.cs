using InsuranceApp.Models;
using InsuranceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Controllers
{
    public class CreatePolicyController : Controller
    {
        private readonly PolicyService _policyService;

        public CreatePolicyController(PolicyService policyService)
        {
            _policyService = policyService;
        }

        // GET: PolicyMvc/Add
        public IActionResult AddPolicy(int partnerId)
        {
            // Inicijaliziraj prazan model Policy i postavi PartnerId
            var policy = new Policy
            {
                PartnerId = partnerId,  // Postavi PartnerId
                // Inicijaliziraj ostale obavezne članove (ako su potrebni)
                PolicyNumber = "",       // Postavi početnu vrijednost ako je potrebno
                PolicyAmount = 0       // Postavi početnu vrijednost ako je potrebno
            };

            ViewBag.PartnerId = partnerId;

            return View(policy); // Prosljeđuje inicijalizirani model u View
        }

        // POST: PolicyMvc/Add
        [HttpPost]
        public async Task<IActionResult> AddPolicy(Policy policy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(policy); // Ako podaci nisu validni, vraća obrazac sa trenutnim podacima
                }

                // Provjerite postoji li polica s istim brojem
                var existingPolicy = await _policyService.GetPolicyByNumberAsync(policy.PolicyNumber);
                if (existingPolicy != null)
                {
                    ModelState.AddModelError("PolicyNumber", "Policy number already exists.");
                    return View(policy);
                }

                // Dodajte novu policu u bazu
                await _policyService.AddPolicyAsync(policy);
                return RedirectToAction("Index", "Home"); // Redirektanje na početnu stranicu ili neku drugu
            }
            catch (Exception ex)
            {
                // Ako dođe do greške, logiraj ili prikazuj poruku
                ModelState.AddModelError("", $"An error occurred while adding the policy: {ex.Message}");
                return View(policy);
            }
        }
    }
}

