using System.ComponentModel.DataAnnotations;
namespace InsuranceApp.Models;

public class City
{
    public int CityId { get; set; }

    public required string PostCode { get; set; }

    [StringLength(40)] 
    public required string CityName { get; set; }

    public int StateId { get; set; }

    
}
