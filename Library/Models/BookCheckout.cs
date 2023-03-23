namespace Library.Models
{
  public class BookCheckout
    {       
        public int BookCheckoutId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
    }
}