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
            // Initialize an empty Policy model and set PartnerId
            var policy = new Policy
            {
                PartnerId = partnerId,  // Set the PartnerId from the passed data
                                        
                PolicyNumber = "",     
                PolicyAmount = 0       
            };

            ViewBag.PartnerId = partnerId;

            return View(policy);        // Pass the initialized model to the View
        }

        // POST: PolicyMvc/Add
        [HttpPost]
        public async Task<IActionResult> AddPolicy(Policy policy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(policy); 
                }

                // Check if a policy with the same policy number already exists
                var existingPolicy = await _policyService.GetPolicyByNumberAsync(policy.PolicyNumber);
                if (existingPolicy != null)
                {
                    ModelState.AddModelError("PolicyNumber", "Policy number already exists.");
                    return View(policy);
                }

                // Add new policy to database
                await _policyService.AddPolicyAsync(policy);
                return RedirectToAction("Index", "Home"); // Redirect to the Home page
            }
            catch (Exception ex)
            {
                // If an error occurs, log it or display a message
                ModelState.AddModelError("", $"An error occurred while adding the policy: {ex.Message}");
                return View(policy);
            }
        }
    }
}

