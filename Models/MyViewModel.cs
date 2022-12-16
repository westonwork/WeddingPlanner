#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace WeddingPlanner.Models;
public class MyViewModel
{
    public User? User {get;set;}
    public Wedding? Wedding {get;set;}

    public List<User> AllUsers {get;set;}
    public List<Wedding> AllWeddings {get;set;}

    public Reservation Reservation {get;set;}
}