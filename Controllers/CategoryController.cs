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
    public class CategoryController : ControllerBase
    {
        public LibiraryContext context;
        public CategoryController(LibiraryContext context) 
        {
            this.context = context;
        }
        //Get All Category
        //[HttpGet]
        //public IActionResult GetAllCategory()
        //{
        //    List<Category> categories = context.Categories.ToList();
        //    return Ok(categories);
        //}

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Category>>> GetAllCateories()
        {
            var categories = await context.Categories
                .Include(c => c.Books)
                .ToListAsync();
            return Ok(categories);
        }


        //AddCategory
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreatecategoreDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync(); // استخدم await

            return CreatedAtAction(nameof(getByID), new { id = category.Id }, category);
        }

        //GetById
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult getByID(int id)
        {
            Category category = context.Categories.FirstOrDefault(c => c.Id == id);
            return Ok(category);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateCategory(int id, UpdateCategoryDTO Dto)
        {
            var category = context.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            category.Name = Dto.Name;
            context.SaveChanges();
            return NoContent();

        }


        //Delete 
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Deltecategory(int id) 
        {
            Category FindCategory = context.Categories.FirstOrDefault((c) => c.Id == id);
            if (FindCategory != null) 
            {
                context.Categories.Remove(FindCategory);
                context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound("Category Not Found");    
            }
        }

    }
}
