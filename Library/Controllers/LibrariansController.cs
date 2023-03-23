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
//using System;

namespace Library.Controllers
{
  [Authorize]
  public class LibrariansController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public LibrariansController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Librarian> model = _db.Librarians.OrderBy(librarian => librarian).ToList();
      //Ranking?
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Librarian librarian)
    {
      if (!ModelState.IsValid)
      {
        return View(librarian);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        librarian.User = currentUser;
        _db.Librarians.Add(librarian);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ViewBag.CurrentUser = userId;

      Librarian thisLibrarian = _db.Librarians
            .Include(librarian => librarian.JoinCatalogLibrarian)
            .ThenInclude(join => join.Catalog)
            .FirstOrDefault(librarian => librarian.LibrarianId == id);
      return View(thisLibrarian);
    }

    public ActionResult Edit(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      return View(thisLibrarian);
    }
    
    [HttpPost]
    public ActionResult Edit(Librarian librarian)
    {
      _db.Librarians.Update(librarian);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      return View(thisLibrarian);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      _db.Librarians.Remove(thisLibrarian);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCatalog(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarians => librarians.LibrarianId == id);
      ViewBag.CatalogId = new SelectList(_db.Catalogs, "CatalogId", "Catalog");
      // what is this page called? ^
      return View(thisLibrarian);
    }

    [HttpPost]
    public ActionResult AddCatalog(Librarian librarian, int catalogId)
    {
      #nullable enable
      CatalogLibrarian? joinEntity = _db.CatalogLibrarians.FirstOrDefault(join => (join.CatalogId == catalogId && join.LibrarianId == librarian.LibrarianId));
      #nullable disable
      if (joinEntity == null && catalogId != 0)
      {
        _db.CatalogLibrarians.Add(new CatalogLibrarian() { CatalogId = catalogId, LibrarianId = librarian.LibrarianId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = librarian.LibrarianId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    // joinId?? what is?^
    {
      CatalogLibrarian joinEntry = _db.CatalogLibrarians.FirstOrDefault(entry => entry.CatalogLibrarianId == joinId);
      _db.CatalogLibrarians.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    // public async Task<ActionResult> Ranking()
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Librarian> model = _db.Librarians.OrderBy(librarian => librarian.Ranking).ToList();
    //   return View(model);
    // }

    // [HttpPost, ActionName("Search")]
    // public ActionResult Search(string search)
    // {
    //   List<Librarian> model = _db.Librarians.Where(recipe => recipe.Ingredients.ToLower()
    //                           .Contains(search.ToLower())).ToList();
    //   return View(model);
    // }
  }
}
