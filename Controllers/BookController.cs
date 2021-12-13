using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReLogin.Data;
using ReLogin.Models;
using ReLogin.Models.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService Books;

        public BookController(IBookService bookRepo)
        {
            Books = bookRepo;
        }
        [HttpGet("get-books")]
        public async Task<IEnumerable<Books>> GetBooks()
        {
            return await Books.Get();
        }
        [HttpGet("{id}")]
        public async Task<Books> GetBook(int id)
        {
            return await Books.Get(id);
        }
        [HttpPost("post-books")]
        public async Task<ActionResult<Books>> Post([FromBody] Books book)
        {
            book.users.Add(new Models.User
            {
                Id = 1,

            });
            var newBook = await Books.Create(book);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
        }
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Books book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            await Books.Update(book);
            return NoContent();
        }
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await Books.Get(id);
            if (bookToDelete == null)
                return NotFound();
            await Books.Delete(bookToDelete.Id); //delete method in repo take id like a param
            return NoContent();
        }
        //public List<Book> getBookList()
        //{

        //    return (from c in _bookRepo
        //            where c.IsDeleted == false
        //
        //  select c).ToList();
        //}
        [HttpPost("getbooks-filter")]
        public async Task<IActionResult> CustomSearch(SearchFilterPaging searchFilterPaging)
        {
            return Ok(await Books.CustomSearch(searchFilterPaging));
        }
        [HttpPost("get-Paged-books")]
        public async Task<IActionResult> GetAll(BaseFilterPaging filter)
        {
            return Ok(await Books.GetPagedBooksAsync(filter));
        }
    } 
}
