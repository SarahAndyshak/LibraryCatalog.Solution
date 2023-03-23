using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Librarian> Librarians { get; set; }

    // join tables
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<BookCatalog> BookCatalogs { get; set; }
    public DbSet<BookCheckout> BookCheckouts { get; set; }
    public DbSet<BookPatron> BookPatrons { get; set; }
    public DbSet<CatalogLibrarian> CatalogLibrarians { get; set; }
    public DbSet<CheckoutPatron> CheckoutPatrons { get; set; }
    public LibraryContext(DbContextOptions options) : base(options) { }
  }
}