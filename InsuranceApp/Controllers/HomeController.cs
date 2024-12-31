using InsuranceApp.Services;
using InsuranceApp.DTOs; // Dodaj namespace za DTO
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly PartnerService _partnerService;

        public HomeController(PartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        // Akcija za prikaz popisa svih partnera
        public async Task<IActionResult> Index()
        {
            // Dohvat svih partnera iz servisa
            var partners = await _partnerService.GetAllPartnersAsync();

            // Definiraj mapu za PartnerTypeId
            var partnerTypeMap = new Dictionary<int, string>
            {
                { 1, "Personal" },
                { 2, "Legal" }
            };

            // Mapiranje Partner modela na PartnerDTO za prikaz na View
            var partnerDtos = partners.Select(p => new PartnerDTO
            {
                PartnerId = p.PartnerId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PartnerNumber = p.PartnerNumber,
                CroatianPIN = p.CroatianPIN,
                PartnerTypeId = p.PartnerTypeId,
                PartnerTypeName = partnerTypeMap[p.PartnerTypeId], // Mapiramo PartnerTypeId na tekst (Personal ili Legal)
                IsForeign = p.IsForeign,
                Gender = p.Gender,
                CreatedAtUtc = p.CreatedAtUtc
            }).ToList(); // Ovime se LINQ rezultat pretvara u stvarnu listu

            // Prosljeđivanje partnerDTO u View
            return View(partnerDtos);
        }

        // Akcija za dohvat detalja partnera
        public async Task<IActionResult> Details(int partnerId)
        {
            // Dohvat partnera iz servisa
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

            if (partner == null)
            {
                return NotFound();
            }

            // Kreiranje PartnerDetailDTO za detalje partnera
            var partnerDetailDto = new PartnerDetailDTO
            {
                Id = partner.PartnerId,
                FirstName = partner.FirstName,
                LastName = partner.LastName,
                Address = partner.Address ?? "", // Ako adresa nije postavljena, koristi prazan string
                PartnerNumber = partner.PartnerNumber,
                CroatianPIN = partner.CroatianPIN,
                PartnerTypeId = partner.PartnerTypeId,
                CreatedAtUtc = partner.CreatedAtUtc,
                CreateByUser = "admin@example.com", // Automatski postavimo admin@example.com kao korisnika
                IsForeign = partner.IsForeign,
                ExternalCode = partner.ExternalCode,
                Gender = partner.Gender,
                CityId = partner.CityId
            };

            // Vraćanje DTO kao JSON
            return Json(partnerDetailDto);
        }
    }
}
