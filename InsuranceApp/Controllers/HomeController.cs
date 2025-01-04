using InsuranceApp.Services;
using InsuranceApp.DTOs; // Dodaj namespace za DTO
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InsuranceApp.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly PartnerService _partnerService;

        // Constructor for injecting the PartnerService
        public HomeController(PartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        // Action to display a list of all partners
        public async Task<IActionResult> Index()
        {
            // Fetch all partners from the service
            var partners = await _partnerService.GetAllPartnersAsync();

            // Define a map for PartnerTypeId
            var partnerTypeMap = new Dictionary<int, string>
            {
                { 1, "Personal" },
                { 2, "Legal" }
            };

            // Map Partner model to PartnerDTO for display in the View
            var partnerDtos = (await Task.WhenAll(partners.Select(async p => new PartnerDTO
            {
                PartnerId = p.PartnerId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PartnerNumber = p.PartnerNumber,
                CroatianPIN = p.CroatianPIN,
                PartnerTypeId = p.PartnerTypeId,
                PartnerTypeName = partnerTypeMap[p.PartnerTypeId], // Map PartnerTypeId to text (Personal or Legal)
                IsForeign = p.IsForeign,
                Gender = p.Gender,
                CreatedAtUtc = p.CreatedAtUtc,
                TotalPolicies = await _partnerService.GetPolicyCountByPartnerIdAsync(p.PartnerId), 
                TotalPolicyAmount = await _partnerService.GetPolicyTotalAmountByPartnerIdAsync(p.PartnerId) 
            }))).ToList(); // Convert the asynchronous result to a concrete list

            // Sort the list by CreatedAtUtc, from newest to oldest
            var sortedPartnerDtos = partnerDtos.OrderByDescending(p => p.CreatedAtUtc).ToList();

            // Pass the partnerDTO to the View
            return View(sortedPartnerDtos);
        }

        // Action to fetch partner details
        public async Task<IActionResult> Details(int partnerId)
        {
            // Fetch the partner from the service
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

            if (partner == null)
            {
                return NotFound();
            }

            // Create PartnerDetailDTO for partner details
            var partnerDetailDto = new PartnerDetailDTO
            {
                Id = partner.PartnerId,
                FirstName = partner.FirstName,
                LastName = partner.LastName,
                Address = partner.Address ?? "", // If address is not set, use an empty string
                PartnerNumber = partner.PartnerNumber,
                CroatianPIN = partner.CroatianPIN,
                PartnerTypeId = partner.PartnerTypeId,
                CreatedAtUtc = partner.CreatedAtUtc,
                CreateByUser = "admin@example.com", // Automatically set admin@example.com as the user
                IsForeign = partner.IsForeign,
                ExternalCode = partner.ExternalCode,
                Gender = partner.Gender,
                CityId = partner.CityId,
                TotalPolicies = await _partnerService.GetPolicyCountByPartnerIdAsync(partner.PartnerId), 
                TotalPolicyAmount = await _partnerService.GetPolicyTotalAmountByPartnerIdAsync(partner.PartnerId) 
            };

            // Return DTO kao JSON
            return Json(partnerDetailDto);
        }
    }
}
