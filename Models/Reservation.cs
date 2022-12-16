#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    // connecting to the user and wedding tables
    [Required]
    public int UserId { get; set; }
    [Required]
    public int WeddingId { get; set; }

// navigation properties
    public User? User { get; set; }
    public Wedding? Wedding { get; set; }

}