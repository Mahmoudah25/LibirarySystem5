using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        public LibiraryContext context;
        public BorrowingController(LibiraryContext context)
        {
            this.context = context;
        }

        //GetAllBorrowing
        [Authorize]
        [HttpGet]
        public IActionResult GetAllBorrowing()
        {
            List<Borrowing> borrowings = context.borrowings.ToList();
            return Ok(borrowings);
        }

        //Add Borrowing
        [HttpPost]
        public IActionResult AddBorrowing(Borrowing borrowing)
        {
            context.borrowings.Add(borrowing);
            context.SaveChanges();
            return CreatedAtAction("GetById", new { borrowing.Id }, borrowing);
        }

        //GetById
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            Borrowing borrowing = context.borrowings.FirstOrDefault(x => x.Id == id);
            return Ok(borrowing);
        }

        //Update
        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateBorrowing(int id, [FromBody] Borrowing borrowing)
        {
            var existingBorrowing = context.borrowings.FirstOrDefault(e => e.Id == id);
            if (existingBorrowing == null)
                return NotFound("Borrowing not found");
            existingBorrowing.BookId = borrowing.BookId;
            existingBorrowing.BorrowDate = borrowing.BorrowDate;
            existingBorrowing.RetuenDate = borrowing.RetuenDate;

            context.SaveChanges();

            return NoContent();
        }


    }
}
