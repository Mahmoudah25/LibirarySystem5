using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentManagmentSystem.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishYear {  get; set; }

        //Fk
        [ForeignKey("Author")]
        public int AuthorId {  get; set; }
        [ForeignKey("Category")]
        public int CategoryId {  get; set; }


        public Author? Author { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public ICollection<Borrowing>? borrowings { get; set; }

    }
}
