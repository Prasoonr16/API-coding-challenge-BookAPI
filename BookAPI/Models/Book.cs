using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models
{
    public class Book
    {
        [Required]
        [Key]
        public int Id{ get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        public string ISBN {  get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Please enter a valid year in YYYY format")]
        public int PublicationYear { get; set; }
    }
}
