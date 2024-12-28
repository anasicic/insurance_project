using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.DTOs;

public class PartnerDTO
{
    public int PartnerId { get; set; }

    [StringLength(255, MinimumLength = 2)]
    [Required]
    public string FirstName { get; set; }= "";

    [StringLength(255, MinimumLength = 2)]
    [Required]
    public string LastName { get; set; }= "";

    [StringLength(255, MinimumLength = 2)]  
    public string FullName => $"{FirstName} {LastName};";

    [StringLength(20, MinimumLength = 20)] 
    public required string PartnerNumber { get; set; }

    [RegularExpression("^[MFN]$")] 
    public required char Gender { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow; 
}
