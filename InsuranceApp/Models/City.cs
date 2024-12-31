using System.ComponentModel.DataAnnotations;
namespace InsuranceApp.Models;

public class City
{
    public int CityId { get; set; } = 1;

    [StringLength(40)] 
    public required string CityName { get; set; }

    public int StateId { get; set; }

    
}
