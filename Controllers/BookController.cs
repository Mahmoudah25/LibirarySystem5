using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagmentSystem.DTO;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibiraryContext context;
        [ActivatorUtilitiesConstructor]
        public BookController(LibiraryContext context)
        {
            this.context = context;
        }
        public BookController() 
        {

        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await context.Books
                .Include(d => d.Author)
                .Include(d=>d.borrowings)
                .Include(d=>d.Category)
                .ToListAsync();

            return Ok(books);
        }

        //AddBook
        [HttpPost]
        public IActionResult AddBook(CreateBookDTO dto) 
        {
            var book = new Book
            {
                Title = dto.Title,  
                ISBN = dto.ISBN,
                PublishYear = dto.PublishYear,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId
            };
            context.Books.Add(book);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }



        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = context.Books.Find(id);
            if (book == null) return NotFound();

            var bookDto = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId
            };

            return Ok(bookDto);
        }

        //GetByName
        [HttpGet]
        [Route("{title:alpha}")]
        public IActionResult GetByName(string title) 
        {
            Book book = context.Books.FirstOrDefault(n => n.Title == title);
            return Ok(book);
        }

      

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, UpdateBookDTO dto) 
        {
            var book = context.Books.Find(id);
            if(book == null) return NotFound();
            book.Title=dto.Title;
            book.ISBN=dto.ISBN;
            book.PublishYear=dto.PublishYear;
            book.AuthorId=dto.AuthorId;
            book.CategoryId=dto.CategoryId;
            context.SaveChanges();
            return NoContent();
        }


        //DeleteBook
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            Book BoDeleted = context.Books.FirstOrDefault(e => e.Id == id);
            if(BoDeleted != null)
            {
                context.Remove(BoDeleted);
                context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound("Book not found");
            }
        }

    }
}
