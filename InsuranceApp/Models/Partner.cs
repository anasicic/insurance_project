using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace InsuranceApp.Models;

public class Partner
{        
    [JsonIgnore]
    public int PartnerId { get; set; }  

    [StringLength(255, MinimumLength = 2)]  
    public required string FirstName { get; set; } = string.Empty;

    [StringLength(255, MinimumLength = 2)]  
    public required string LastName { get; set; } 
 
    public string Address { get; set; } = string.Empty;
    
    [StringLength(20, MinimumLength = 20)]  
    public required string PartnerNumber { get; set; } 

    [StringLength(11, MinimumLength = 11)]
    public string CroatianPIN { get; set; }= string.Empty;

    public required int PartnerTypeId { get; set; } = 1;
    [JsonIgnore]
    public DateTime CreatedAtUtc { get; set; } // Inicijalizacija samo kad se unosi  
 
    [EmailAddress]  
    [StringLength(255)]  
    public required string CreateByUser { get; set; } = "admin@example.com"; // Default value here


    public required bool IsForeign { get; set; }  

    [StringLength(20, MinimumLength = 10, ErrorMessage = "ExternalCode must be between 10 and 20 characters.")]  
    public required string ExternalCode { get; set; } 

    [RegularExpression("^[MFN]$")] 
    public required char Gender { get; set; } 
  
    public required int CityId { get; set; } = 1;

    public int TotalPolicies { get; set; }
    public decimal TotalPolicyAmount { get; set; } 

    
}
