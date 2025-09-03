using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentManagmentSystem.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Bio {  get; set; }
        [JsonIgnore]

        public ICollection<Book>? Books { get; set; }
    }
}
