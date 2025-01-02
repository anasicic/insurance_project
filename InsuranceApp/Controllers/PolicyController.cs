using InsuranceApp.Models;
using InsuranceApp.Services;
using Microsoft.AspNetCore.Mvc;


namespace InsuranceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly PolicyService _policyService;

        public PolicyController(PolicyService policyService)
        {
            _policyService = policyService;
        }

        // Get all policies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Policy>>> GetAllPoliciesAsync()
        {
            try
            {
                var policies = await _policyService.GetAllPoliciesAsync();
                return Ok(policies);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching all policies: {ex.Message}");
                // Throw the exception so it can be handled by global exception handler or middleware
                return StatusCode(500, "An error occurred while retrieving policies."); // Return status code 500 for error
            }
            }
        

        // Get policies by PartnerId
        [HttpGet("{partnerId}")]
        public async Task<IActionResult> GetPoliciesByPartnerId(int partnerId)
        {
            try
            {
                var policies = await _policyService.GetPoliciesByPartnerIdAsync(partnerId);
                if (policies == null || !policies.Any())
                {
                    return NotFound($"No policies found for partner with ID {partnerId}");
                }

                return Ok(policies);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching policies for partnerId {partnerId}: {ex.Message}");
                // Throw the exception so it can be handled by global exception handler or middleware
                return StatusCode(500, "An error occurred while retrieving policies for the specified partner.");
            }
            }
        

        // Add a new policy
        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] Policy policy)
        {
            try
            {
                // Check the validity of the model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if a policy with the same number already exists
                var existingPolicy = await _policyService.GetPolicyByNumberAsync(policy.PolicyNumber);
                if (existingPolicy != null)
                {
                    // If a policy with the same number exists, return status 409 Conflic
                    return Conflict("Policy number already exists.");
                }

                // If everything is fine, add the new policy
                await _policyService.AddPolicyAsync(policy);

                // Return status 201 Created and return the details of the new policy
                return CreatedAtAction(nameof(GetPoliciesByPartnerId), new { partnerId = policy.PartnerId }, policy);
            }
            catch (Exception ex)
            {
                // If an error occurs, log the exception and return status 500
                Console.WriteLine($"Error adding policy: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the policy.");
            }
        }
    }
}
