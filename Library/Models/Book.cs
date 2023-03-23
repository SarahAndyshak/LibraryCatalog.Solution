using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        // public string Author { get; set; }
        public List<AuthorBook> JoinAuthorBook { get;}
        public List<BookCatalog> JoinBookCatalog { get;}

        public List<BookCheckout> JoinBookCheckout { get; }
        // public List<CheckoutPatron> JoinCheckoutPatron { get; }
        public ApplicationUser User { get; set; } 
    }
}