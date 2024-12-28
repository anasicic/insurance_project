using InsuranceApp.Services;
using InsuranceApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace InsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly PartnerService _partnerService;

        public PartnerController(PartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        // GET: api/partners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partner>>> GetPartners()
        {
            try
            {
                // Fetching partners from the service.
                var partners = await _partnerService.GetAllPartnersAsync();
        
                // If there are no partners, it returns a 404 Not Found.
                if (partners == null || !partners.Any())
                {
                    return NotFound("No partners found.");
                }

                // It returns HTTP 200 with the list of partners.
                return Ok(partners);
            }
            catch (Exception ex)
            {
                // Error handling - it returns a 500 status with an error message.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/partners/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Partner>> GetPartnerById(int id)
        {
            try
            {
                var partner = await _partnerService.GetPartnerByIdAsync(id);
                if (partner == null)
                {
                    // Ako partner nije pronađen, vraća HTTP 404
                    return NotFound($"Partner with ID {id} not found.");
                }
                // Vraća HTTP 200 sa partnerom
                return Ok(partner);
            }
            catch (Exception ex)
            {
                // Rukovanje greškom - vraća status 500 sa porukom greške
                return StatusCode(500, $"Error fetching partner by ID {id}: {ex.Message}");
            }
        }

        // POST: api/partner
        [HttpPost]
        public async Task<ActionResult<Partner>> AddPartner([FromBody] Partner partner)
        {
            if (partner == null)
            {
                // If the data is not valid, it returns HTTP 400 Bad Request.
                return BadRequest("Partner data is required.");
            }

            try
            {
                await _partnerService.AddPartnerAsync(partner);
                // Returns HTTP 201 Created with the details of the new partner.
                return CreatedAtAction(nameof(GetPartnerById), new { id = partner.PartnerId }, partner);
            }
            catch (Exception ex)
            {
                // Error handling - returns status 500 with an error message.
                return StatusCode(500, $"Error adding partner: {ex.Message}");
            }
        }
    }
}
