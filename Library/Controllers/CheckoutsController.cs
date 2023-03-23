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
  public class CheckoutsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Checkout> model = _db.Checkouts.OrderBy(checkout => checkout.History).ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Checkout checkout)
    {
      if (!ModelState.IsValid)
      {
        return View(checkout);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        checkout.User = currentUser;
        _db.Checkouts.Add(checkout);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
// This one works, do not delete
    public ActionResult Details(int id)
    {
      Checkout thisCheckout = _db.Checkouts
            .Include(checkout => checkout.JoinBookCheckout)
            .ThenInclude(join => join.Book)
            .FirstOrDefault(checkout => checkout.CheckoutId == id);
      return View(thisCheckout);
    }

    public ActionResult Edit(int id)
    {
      Checkout thisCheckout = _db.Checkouts.FirstOrDefault(checkout => checkout.CheckoutId == id);
      return View(thisCheckout);
    }

    [HttpPost]
    public ActionResult Edit(Checkout checkout)
    {
      _db.Checkouts.Update(checkout);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Checkout thisCheckout = _db.Checkouts.FirstOrDefault(checkout => checkout.CheckoutId == id);
      return View(thisCheckout);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Checkout thisCheckout = _db.Checkouts.FirstOrDefault(checkout => checkout.CheckoutId == id);
      _db.Checkouts.Remove(thisCheckout);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddBook(int id)
    {
      Checkout thisCheckout = _db.Checkouts.FirstOrDefault(checkouts => checkouts.CheckoutId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisCheckout);
    }

    [HttpPost]
    public ActionResult AddBook(Checkout checkout, int bookId)
    {
      #nullable enable
      BookCheckout? joinEntity = _db.BookCheckouts.FirstOrDefault(join => (join.BookId == bookId && join.CheckoutId == checkout.CheckoutId));
      #nullable disable
      if (joinEntity == null && bookId != 0)
      {
        _db.BookCheckouts.Add(new BookCheckout() { BookId = bookId, CheckoutId = checkout.CheckoutId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = checkout.CheckoutId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      BookCheckout joinEntry = _db.BookCheckouts.FirstOrDefault(entry => entry.BookCheckoutId == joinId);
      _db.BookCheckouts.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public async Task<ActionResult> History()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Checkout> model = _db.Checkouts.OrderBy(checkout => checkout.History).ToList();
      return View(model);
    }

    [HttpPost, ActionName("Search")]
    public ActionResult Search(string search)
    {
      List<Checkout> model = _db.Checkouts.Where(checkout => checkout.History.ToLower()
                              .Contains(search.ToLower())).ToList();
      return View(model);
    }
  }
}