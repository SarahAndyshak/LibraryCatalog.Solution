using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
      private readonly LibraryContext _db;
      private readonly UserManager<ApplicationUser> _userManager;

      public HomeController(UserManager<ApplicationUser> userManager, LibraryContext db)
      {
        _userManager = userManager;
        _db = db;
      }
      
    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      
      //Recipe logic
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      if (currentUser != null)
      {
        Checkout[] checkouts = _db.Checkouts
                        .Where(entry => entry.User.Id == currentUser.Id)
                        .ToArray();
        model.Add("checkouts", checkouts);
      }
      return View(model);
    }
  }
}