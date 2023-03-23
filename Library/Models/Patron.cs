using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Patron
    {
        public int PatronId { get; set; }
        public string PatronName { get; set; }
        public List<BookPatron> JoinBookPatron { get;}
        public List<CheckoutPatron> JoinCheckoutPatron { get; }
        public ApplicationUser User { get; set; } 
    }
}