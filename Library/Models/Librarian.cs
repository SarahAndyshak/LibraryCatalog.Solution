using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Librarian
    {
        public int LibrarianId { get; set; }
        public string LibrarianName { get; set; }
        public List<CatalogLibrarian> JoinCatalogLibrarian { get; }
        public ApplicationUser User { get; set; } 
    }
}