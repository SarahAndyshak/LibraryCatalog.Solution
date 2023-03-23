using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Checkout
    {
        public int CheckoutId { get; set; }
        public string History { get; set; }
        public List<BookCheckout> JoinBookCheckout { get;}
        public List<CheckoutPatron> JoinCheckoutPatron { get; }
        public ApplicationUser User { get; set; } 
    }
}