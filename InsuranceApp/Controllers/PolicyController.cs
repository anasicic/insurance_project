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
                throw new InvalidOperationException("An error occurred while retrieving policies.", ex);
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
                throw new InvalidOperationException($"An error occurred while retrieving policies for partner ID {partnerId}.", ex);
            }
        }

        // Add a new policy
        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] Policy policy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _policyService.AddPolicyAsync(policy);
                return CreatedAtAction(nameof(GetPoliciesByPartnerId), new { partnerId = policy.PartnerId }, policy);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding policy: {ex.Message}");
                // Throw the exception to be handled by global exception handler or middleware
                throw new InvalidOperationException("An error occurred while adding the policy.", ex);
            }
        }
    }
}
