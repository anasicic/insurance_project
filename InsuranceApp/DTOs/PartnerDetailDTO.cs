using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.DTOs;

public class PartnerDetailDTO
{
    public int Id { get; set; }

    [StringLength(255, MinimumLength = 2)]
    public required string FirstName { get; set; }

    [StringLength(255, MinimumLength = 2)]
    public required string LastName { get; set; }

    [StringLength(255, MinimumLength = 2)]  
    public string FullName => $"{FirstName} {LastName}";

    public string Address { get; set; } ="";

    [StringLength(20, MinimumLength = 20)]
    public required string PartnerNumber { get; set; }

    public string CroatianPIN { get; set; } ="";

    public required int PartnerTypeId { get; set; }

    public string PartnerTypeName { get; set; }="";

    public required  DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [EmailAddress]
    [StringLength(255)]
    public required string CreateByUser { get; set; }

    public required bool IsForeign { get; set; }

    [StringLength(20, MinimumLength = 10)]
    public required string ExternalCode { get; set; }

    [RegularExpression("^[MFN]$")]
    public required char Gender { get; set; }

    public required int CityId { get; set; }

    public string CityName { get; set; } ="";

    public int TotalPolicies { get; set; }
    public decimal TotalPolicyAmount { get; set; } 

}
