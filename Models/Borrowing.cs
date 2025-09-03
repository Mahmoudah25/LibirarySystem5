using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentManagmentSystem.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        //Fk
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? RetuenDate { get; set; }
        [JsonIgnore]
        public Book? Book { get; set; }
    }
}
