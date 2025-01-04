using System.ComponentModel.DataAnnotations;
namespace InsuranceApp.Models;

public class City
{
    public int CityId { get; set; } = 0;

    [StringLength(40)] 
    public required string CityName { get; set; } = string.Empty;

    public int StateId { get; set; }

    
}
