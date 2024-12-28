using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InsuranceApp.Models;

public class Policy
{
    [JsonIgnore]
    public int PolicyId { get; set; }

    [StringLength(15, MinimumLength = 10)] 
    public required string PolicyNumber { get; set; }

    public required decimal PolicyAmount { get; set; }

    public int PartnerId { get; set; }
 

}
