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
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Book> model = _db.Books.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Book book)
    {
      if (!ModelState.IsValid)
      {
        return View(book);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        book.User = currentUser;
        _db.Books.Add(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

// This one works, do not delete
    public ActionResult Details(int id)
    {
      Book thisBook = _db.Books
            .Include(book => book.JoinAuthorBook)
            .ThenInclude(join => join.Author)
            .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    public ActionResult Edit(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Books.Update(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// Add Author Join and Delete Author Join
    public ActionResult AddAuthor(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "AuthorName");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int authorId)
    {
      #nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => (join.AuthorId == authorId && join.BookId == book.BookId)); //joinEntity is just the variable name
      #nullable disable
      if (joinEntity == null && authorId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = book.BookId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

// Add Checkout Join and Delete Checkout Join
    public ActionResult AddCheckout(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
      ViewBag.CheckoutId = new SelectList(_db.Checkouts, "CheckoutId", "History");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddCheckout(Book book, int checkoutId)
    {
      #nullable enable
      BookCheckout? joinEntity = _db.BookCheckouts.FirstOrDefault(join => (join.CheckoutId == checkoutId && join.BookId == book.BookId)); //joinEntity is just the variable name
      #nullable disable
      if (joinEntity == null && checkoutId != 0)
      {
        _db.BookCheckouts.Add(new BookCheckout() { CheckoutId = checkoutId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = book.BookId });
    }

    [HttpPost]
    public ActionResult DeleteCheckoutJoin(int joinId) // may need to be renamed, or get an action name -- can't have two DeleteJoin methods
    {
      BookCheckout joinEntry = _db.BookCheckouts.FirstOrDefault(entry => entry.BookCheckoutId == joinId);
      _db.BookCheckouts.Remove(joinEntry);
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
    
// Search by Title
    [HttpPost, ActionName("SearchTitle")]
    public ActionResult Search(string search)
    {
      List<Book> model = _db.Books.Where(book => book.Title.ToLower()
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