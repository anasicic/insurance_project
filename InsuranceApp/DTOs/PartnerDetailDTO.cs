using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.DTOs;

public class PartnerDetailDTO
{
    public int Id { get; set; }

    [StringLength(255, MinimumLength = 2)]
    public required string FirstName { get; set; } = string.Empty;

    [StringLength(255, MinimumLength = 2)]
    public required string LastName { get; set; } = string.Empty;

    [StringLength(255, MinimumLength = 2)]  
    public string FullName => $"{FirstName} {LastName}";

    public string Address { get; set; } = string.Empty;

    [StringLength(20, MinimumLength = 20)]
    public required string PartnerNumber { get; set; }  = string.Empty;

    public string CroatianPIN { get; set; } = string.Empty;

    public required int PartnerTypeId { get; set; }

    public string PartnerTypeName { get; set; }= string.Empty;

    public required  DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [EmailAddress]
    [StringLength(255)]
    public required string CreateByUser { get; set; } = string.Empty;

    public required bool IsForeign { get; set; }

    [StringLength(20, MinimumLength = 10)]
    public required string ExternalCode { get; set; } = string.Empty;

    [RegularExpression("^[MFN]$")]
    public required char Gender { get; set; } 

    public required int CityId { get; set; }

    public string CityName { get; set; } = string.Empty;

    public int TotalPolicies { get; set; }
    public decimal TotalPolicyAmount { get; set; } 

}
