using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<AuthorBook> JoinAuthorBook { get;}
        public ApplicationUser User { get; set; } 
    }
}