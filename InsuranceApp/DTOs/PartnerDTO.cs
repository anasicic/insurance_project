using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.DTOs;

public class PartnerDTO
{
    public int PartnerId { get; set; }

    [StringLength(255, MinimumLength = 2)]
    public required string FirstName { get; set; }= "";

    [StringLength(255, MinimumLength = 2)]
    public required string LastName { get; set; }= "";

    [StringLength(255, MinimumLength = 2)]  
    public string FullName => $"{FirstName} {LastName}";

    [StringLength(20, MinimumLength = 20)] 
    public required string PartnerNumber { get; set; }

    [StringLength(11, MinimumLength = 11)]
    public string CroatianPIN { get; set; } = "";
    
    public required int PartnerTypeId { get; set; }
    
    public required string PartnerTypeName { get; set; }="";

    public required bool IsForeign { get; set; } 

    [RegularExpression("^[MFN]$")] 
    public required char Gender { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow; 

    public int TotalPolicies { get; set; }
    public decimal TotalPolicyAmount { get; set; }
    
}
