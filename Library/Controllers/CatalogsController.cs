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
  public class CatalogsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CatalogsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // List<Catalog> model = _db.Catalogs.OrderBy(catalog => catalog.Ranking).ToList();
      return View();
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Catalog catalog)
    {
      if (!ModelState.IsValid)
      {
        return View(catalog);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        catalog.User = currentUser;
        _db.Catalogs.Add(catalog);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

// This one works, do not delete
    public ActionResult Details(int id)
    {
      Catalog thisCatalog = _db.Catalogs
            .Include(catalog => catalog.JoinBookCatalog)
            .ThenInclude(join => join.Book)
            .FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }

    public ActionResult Edit(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }

    [HttpPost]
    public ActionResult Edit(Catalog catalog)
    {
      _db.Catalogs.Update(catalog);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalog => catalog.CatalogId == id);
      return View(thisCatalog);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalog => catalog.CatalogId == id);
      _db.Catalogs.Remove(thisCatalog);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddBook(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalogs => catalogs.CatalogId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisCatalog);
    }

    [HttpPost]
    public ActionResult AddBook(Catalog catalog, int bookId)
    {
      #nullable enable
      BookCatalog? joinEntity = _db.BookCatalogs.FirstOrDefault(join => (join.BookId == bookId && join.CatalogId == catalog.CatalogId));
      #nullable disable
      if (joinEntity == null && bookId != 0)
      {
        _db.BookCatalogs.Add(new BookCatalog() { BookId = bookId, CatalogId = catalog.CatalogId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = catalog.CatalogId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      BookCatalog joinEntry = _db.BookCatalogs.FirstOrDefault(entry => entry.BookCatalogId == joinId);
      _db.BookCatalogs.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddLibrarian(int id)
    {
      Catalog thisCatalog = _db.Catalogs.FirstOrDefault(catalogs => catalogs.CatalogId == id);
      ViewBag.LibrarianId = new SelectList(_db.Librarians, "LibrarianId", "LibrarianName");
      return View(thisCatalog);
    }

    [HttpPost]
    public ActionResult AddLibrarian(Catalog catalog, int librarianId)
    {
      #nullable enable
      CatalogLibrarian? joinEntity = _db.CatalogLibrarians.FirstOrDefault(join => (join.LibrarianId == librarianId && join.CatalogId == catalog.CatalogId));
      #nullable disable
      if (joinEntity == null && librarianId != 0)
      {
        _db.CatalogLibrarians.Add(new CatalogLibrarian() { LibrarianId = librarianId, CatalogId = catalog.CatalogId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = catalog.CatalogId });
    }

    [HttpPost]
    public ActionResult DeleteCatalogLibrarianJoin(int joinId)
    {
      CatalogLibrarian joinEntry = _db.CatalogLibrarians.FirstOrDefault(entry => entry.CatalogLibrarianId == joinId);
      _db.CatalogLibrarians.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
// This is what we want: searching through ingredients
    [HttpPost, ActionName("Search")]
    public ActionResult Search(string search)
    {
      List<Catalog> model = _db.Catalogs.Where(catalog => catalog.Quantity.ToLower()
                              .Contains(search.ToLower())).ToList();
      return View(model);
    }
  }
}