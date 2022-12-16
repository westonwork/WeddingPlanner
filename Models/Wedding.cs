#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class Wedding
{
    [Key]
    public int WeddingId {get;set;}
    [Required]
    public string WedderOne {get;set;}
    [Required]
    public string WedderTwo {get;set;}
    [Required]
    [DataType(DataType.Date)]
    [FutureDate]
    public DateTime Date {get;set;}
    [Required]
    public string Address {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

// // One to many
    public int UserId {get;set;}
    public User? Planner {get;set;}
// Many to many
    public List<Reservation> WeddingReservations {get;set;} = new List<Reservation>();

}

public class FutureDateAttribute : ValidationAttribute
{    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {        
        if(value == null) 
        {
            return new ValidationResult("Must have Date");
        }
        if((DateTime)value < DateTime.Now)
        {
            return new ValidationResult("Date must be in the future");
        } else {
            Console.WriteLine(value);
            return ValidationResult.Success;
        }
    }
}