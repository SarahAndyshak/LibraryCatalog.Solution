using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System;

namespace Library.Controllers
{
  [Authorize]
  public class PatronsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatronsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Patron> model = _db.Patrons.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Patron patron)
    {
      if (!ModelState.IsValid)
      {
        return View(patron);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        patron.User = currentUser;
        _db.Patrons.Add(patron);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

// This one works, do not delete
    public ActionResult Details(int id)
    {
      Patron thisPatron = _db.Patrons
            .Include(patron => patron.JoinCheckoutPatron)
            .ThenInclude(join => join.Checkout)
            .FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }

    public ActionResult Edit(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }

    [HttpPost]
    public ActionResult Edit(Patron patron)
    {
      _db.Patrons.Update(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patron => patron.PatronId == id);
      _db.Patrons.Remove(thisPatron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// Add Book Join and Delete Book Join
    public ActionResult AddBook(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patrons => patrons.PatronId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisPatron);
    }

    [HttpPost]
    public ActionResult AddBook(Patron patron, int bookId)
    {
      #nullable enable
      BookPatron? joinEntity = _db.BookPatrons.FirstOrDefault(join => (join.BookId == bookId && join.PatronId == patron.PatronId)); //joinEntity is just the variable name
      #nullable disable
      if (joinEntity == null && bookId != 0)
      {
        _db.BookPatrons.Add(new BookPatron() { BookId = bookId, PatronId = patron.PatronId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = patron.PatronId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      BookPatron joinEntry = _db.BookPatrons.FirstOrDefault(entry => entry.BookPatronId == joinId);
      _db.BookPatrons.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// Add Checkout Join and Delete Checkout Join
    public ActionResult AddCheckout(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patrons => patrons.PatronId == id);
      ViewBag.CheckoutId = new SelectList(_db.Checkouts, "CheckoutId", "History");
      return View(thisPatron);
    }

    [HttpPost]
    public ActionResult AddCheckout(Patron patron, int checkoutId)
    {
      #nullable enable
      CheckoutPatron? joinEntity = _db.CheckoutPatrons.FirstOrDefault(join => (join.CheckoutId == checkoutId && join.PatronId == patron.PatronId)); //joinEntity is just the variable name
      #nullable disable
      if (joinEntity == null && checkoutId != 0)
      {
        _db.CheckoutPatrons.Add(new CheckoutPatron() { CheckoutId = checkoutId, PatronId = patron.PatronId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = patron.PatronId });
    }

    [HttpPost]
    public ActionResult DeleteCheckoutJoin(int joinId) // may need to be renamed, or get an action name -- can't have two DeleteJoin methods
    {
      CheckoutPatron joinEntry = _db.CheckoutPatrons.FirstOrDefault(entry => entry.CheckoutPatronId == joinId);
      _db.CheckoutPatrons.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// Not needed for Library, yet
    // public async Task<ActionResult> Ranking()
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Recipe> model = _db.Recipes.OrderBy(recipe => recipe.Ranking).ToList();
    //   return View(model);
    // }

// This one works! Do not delete! -- Search by book title
    // [HttpPost, ActionName("Search")]
    // public ActionResult Search(string search)
    // {
    //   List<Book> model = _db.Books.Where(book => book.Title == search).ToList();
    //   return View(model);
    // }
    
// Search by Patron Name
    [HttpPost, ActionName("Search")]
    public ActionResult Search(string search)
    {
      List<Patron> model = _db.Patrons.Where(patron => patron.PatronName.ToLower()
                              .Contains(search.ToLower())).ToList();
      return View(model);
    }

// Search by Author -- need different ActionResult? figure out how to deal with AuthorName not exisiting as property of Book
    // [HttpPost, ActionName("SearchAuthor")]
    // public ActionResult Search(string search)
    // {
    //   List<Book> model = _db.Books.Where(book => Book.AuthorName.ToLower()
    //                       .Contains(search.ToLower())).ToList();
    // }
  }
}