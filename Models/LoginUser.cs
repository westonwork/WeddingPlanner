#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class LoginUser
{
    [Required(ErrorMessage = "Email is Required")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string LEmail { get; set; }
    [Required(ErrorMessage = "Password is Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string LPassword { get; set; }
}