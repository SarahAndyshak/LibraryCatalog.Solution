using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Catalog
    {
        public int CatalogId { get; set; }
        public string Quantity { get; set; }
        public List<BookCatalog> JoinBookCatalog { get;}
        public List<CatalogLibrarian> JoinCatalogLibrarian { get; }
        public ApplicationUser User { get; set; } 
    }
}