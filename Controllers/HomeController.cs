using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("users/create")]
    public IActionResult CreateUser(User newUser)
    {
        if(ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Weddings");
        } else {
            return View("Index");
        }
    }

    [HttpPost("users/login")]
    public IActionResult Login(LoginUser userSubmission)
    {
    if(ModelState.IsValid)
        { 
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LEmail);
            if(userInDb == null)
            {
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Index");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LPassword);
            if(result == 0)
            {
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Index");
            }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                HttpContext.Session.SetString("FirstName", userInDb.FirstName);
                return RedirectToAction("Weddings");
        } else {
            return View("Index");
        }
    }

    [SessionCheck]
    [HttpGet("weddings")]
    public IActionResult Weddings()
    {
        string? Username = HttpContext.Session.GetString("Username");
        List<Wedding> allWeddings = _context.Weddings
                                                    .Include(a => a.WeddingReservations)
                                                    .ThenInclude(s => s.User)
                                                    .ToList();
        // MyViewModel MyModel = new MyViewModel
        // {
        //     AllWeddings = allWeddings
        // };
        return View(allWeddings);
    }

    [SessionCheck]
    [HttpGet("weddings/new")]
    public IActionResult NewWedding()
    {
        return View();
    }

    [SessionCheck]
    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if(ModelState.IsValid)
        {
            newWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        } else {
            return View("NewWedding");
        }
    }

    [SessionCheck]
    [HttpGet("weddings/{weddingsId}")]
    public IActionResult OneWedding(int weddingId)
    {
        // So the information on the one craft will be available on the "One" page.
        Wedding? One = _context.Weddings.Include(s => s.Planner).FirstOrDefault(a => a.WeddingId == weddingId);
        ViewBag.OneWedding = One;
        return View(One);
    }

    [HttpGet("weddings/show/{weddingId}")]
    public IActionResult WeddingsOne(int weddingId)
    {
        Wedding? singleWedding = _context.Weddings
                                                .Include(g => g.WeddingReservations)
                                                .ThenInclude(u => u.User)
                                                .FirstOrDefault(w => w.WeddingId == weddingId);
        MyViewModel MyModel = new MyViewModel();
        singleWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
        MyModel.Wedding = singleWedding;
        return View(MyModel);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [HttpPost("weddings/{weddingId}/destroy")]
    public IActionResult DestroyWedding(int weddingId)
    {
        Wedding? WeddingToDestroy = _context.Weddings.SingleOrDefault(a => a.WeddingId == weddingId);
        if(WeddingToDestroy == null)
        {
            return RedirectToAction("Weddings");
        }
        _context.Remove(WeddingToDestroy);
        _context.SaveChanges();
        return RedirectToAction("Weddings");
    }

    [SessionCheck]
    [HttpPost("reservations/create")]
    public IActionResult CreateReservation(Reservation newReservation)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newReservation);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        } else {
            return View("Weddings");
        }
    }

    [SessionCheck]
    [HttpPost("reservations/{reservationId}/destroy")]
    public IActionResult DestroyReservation(int reservationId)
    {
        if(ModelState.IsValid)
        {
            Reservation? ReservationToDestroy = _context.Reservations.SingleOrDefault(a => a.ReservationId == reservationId);
            _context.Remove(ReservationToDestroy);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        } else {
            return View("Weddings");
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
