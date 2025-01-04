using InsuranceApp.Services;
using InsuranceApp.Models;
using Microsoft.AspNetCore.Mvc;
using InsuranceApp.DTOs;

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
                var partners = await _partnerService.GetAllPartnersAsync();
                if (partners == null || !partners.Any())
                {
                    return NotFound("No partners found.");
                }
                return Ok(partners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /Partner/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetPartnerDetails(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
            {
                return NoContent();
            }
            
            var partnerTypeMap = new Dictionary<int, string>
            {
                { 1, "Personal" },
                { 2, "Legal" }
            };

            var cityName = await _partnerService.GetCityNameByIdAsync(partner.CityId);


            var partnerDetailDto = new PartnerDetailDTO
            {
                Id = partner.PartnerId,
                FirstName = partner.FirstName,
                LastName = partner.LastName,
                PartnerNumber = partner.PartnerNumber,
                CroatianPIN = partner.CroatianPIN,
                PartnerTypeId = partner.PartnerTypeId,
                PartnerTypeName = partnerTypeMap[partner.PartnerTypeId],
                IsForeign = partner.IsForeign,
                CreatedAtUtc = partner.CreatedAtUtc,
                CreateByUser = partner.CreateByUser ?? "admin@example.com",
                Gender = partner.Gender,
                CityName = cityName,
                CityId = partner.CityId,
                Address = partner.Address,
                ExternalCode = partner.ExternalCode,
                TotalPolicies = await _partnerService.GetPolicyCountByPartnerIdAsync(partner.PartnerId),
                TotalPolicyAmount = await _partnerService.GetPolicyTotalAmountByPartnerIdAsync(partner.PartnerId)
            };

            return Ok(partnerDetailDto);
        }

        // GET: api/partner/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Partner>> GetPartnerById(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
            {
                return NotFound($"Partner with ID {id} not found.");
            }
            return Ok(partner);
        }

        // POST: api/partner
        [HttpPost("create")]
        public async Task<ActionResult<Partner>> AddPartner([FromBody] Partner partner)
        {
            if (partner == null)
            {
                return BadRequest("Partner data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _partnerService.AddPartnerAsync(partner);
                return CreatedAtAction(nameof(GetPartnerById), new { id = partner.PartnerId }, partner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding partner: {ex.Message}");
            }
        }

        
    }
}
