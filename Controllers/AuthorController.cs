using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagmentSystem.DTO;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        public LibiraryContext context;
        public AuthorController(LibiraryContext context) 
        {
            this.context = context;
        }

        //Get All Author
        [HttpGet]
        public IActionResult GetAllAuthor()
        {
             List<Author> Author = context.Author.ToList();
            return Ok(Author);
        }

        //GetAuthorBtID
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetByID(int id)
        {
            Author auo = context.Author.FirstOrDefault(e => e.Id == id);
            return Ok(auo);
        }


        //GetAuthorBtName
        [HttpGet]
        [Route("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            Author auo = context.Author.FirstOrDefault(e => e.Name == name);
            return Ok(auo);
        }

        //AddAuthor
        [HttpPost]
        public IActionResult AddAuthor([FromBody] CraeteAuthorDTO dto) 
        {
            var author = new  Author
            {
                Name = dto.Name,
                Bio = dto.Bio
            };
            context.Author.Add(author);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetByID), new { id = author.Id},author);
        }

        //Upadete
        [HttpPut("{id:int}")]
        public IActionResult UpdateAuthor(int id, [FromBody] UpadeteAuthorDTO dto) 
        {
            var author = context.Author.Find(id);
            if (author == null) 
            {
                return NotFound();
            }
            author.Name = dto.Name;
            author.Bio =dto.Bio;
            context.SaveChanges();
            return NoContent();

        }

        


        //Delete
        [HttpDelete]
        public IActionResult DeleteAuthor(int id) 
        {
            Author author = context.Author.FirstOrDefault(e=>e.Id==id);
            if (author != null) 
            {
                context.Author.Remove(author);
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
