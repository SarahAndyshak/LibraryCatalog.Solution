namespace Library.Models
{
  public class CheckoutPatron
    {       
        public int CheckoutPatronId { get; set; }
        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
        public int PatronId { get; set; }
        public Patron Patron { get; set; }
    }
}