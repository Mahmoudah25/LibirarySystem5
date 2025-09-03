using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace StudentManagmentSystem.Models
{
    public class LibiraryContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Borrowing>borrowings { get; set; }

        public LibiraryContext(DbContextOptions<LibiraryContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Book → Author (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // Book → Category (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            // Borrowing → Book (Many-to-One)
            modelBuilder.Entity<Borrowing>()
                .HasOne(br => br.Book)
                .WithMany(b => b.borrowings)
                .HasForeignKey(br => br.BookId);
        }
    }
}
