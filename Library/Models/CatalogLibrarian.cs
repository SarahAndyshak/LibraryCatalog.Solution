namespace Library.Models
{
  public class CatalogLibrarian
    {       
        public int CatalogLibrarianId { get; set; }
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public int LibrarianId { get; set; }
        public Librarian Librarian { get; set; }
    }
}