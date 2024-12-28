using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace InsuranceApp.Models;

public class Partner
{        
    [JsonIgnore]
    public int PartnerId { get; set; }  

    [StringLength(255, MinimumLength = 2)]  
    public required string FirstName { get; set; } 

    [StringLength(255, MinimumLength = 2)]  
    public required string LastName { get; set; }

    [StringLength(255)]  
    public string Address { get; set; } ="";
    
    [StringLength(20, MinimumLength = 20)]  
    public required string PartnerNumber { get; set; } 

    public string CroatianPIN { get; set; }="";

    [Required]  
    public int PartnerTypeId { get; set; } 
    [JsonIgnore]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;  
 
    [EmailAddress]  
    [StringLength(255)]  
    public required string CreateByUser { get; set; } 

    public required bool IsForeign { get; set; }  

    [StringLength(20, MinimumLength = 10)]  
    public required string ExternalCode { get; set; } 

    [RegularExpression("^[MFN]$")] 
    public required char Gender { get; set; } 
  
    public required int CityId { get; set; }  

    
}
