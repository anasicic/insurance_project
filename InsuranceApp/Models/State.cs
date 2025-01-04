using System.ComponentModel.DataAnnotations;


namespace InsuranceApp.Models;

public class State
{
    public int StateId { get; set; }
    
    [StringLength(40)] 
    public required string StateName { get; set; } = string.Empty;
    
}
