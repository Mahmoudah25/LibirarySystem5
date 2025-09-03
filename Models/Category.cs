using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace StudentManagmentSystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
