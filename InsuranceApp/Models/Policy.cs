using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InsuranceApp.Models;

public class Policy
{
    [JsonIgnore]
    public int PolicyId { get; set; }

    [StringLength(15, MinimumLength = 10)] 
    public required string PolicyNumber { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public required decimal PolicyAmount { get; set; }

    public int PartnerId { get; set; }
 

}
