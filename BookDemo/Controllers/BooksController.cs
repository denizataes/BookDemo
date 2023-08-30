using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookDemo.Data;
using BookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookDemo.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ApplicationContext.Books);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute(Name = "id")] int id)
        {

            var item = ApplicationContext.Books.Where(x => x.Id == id).SingleOrDefault();
            return item is not null ? Ok(item) : NotFound();
        }

        // POST api/values
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest();
                }
                ApplicationContext.Books.Add(book);

                return StatusCode(201, book);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            var entity = ApplicationContext.Books.Where(x => x.Id == id).FirstOrDefault();
            if (entity is not null)
            {
                if (id != book.Id)
                    return BadRequest();

                ApplicationContext.Books.Remove(entity);
                book.Id = entity.Id;
                ApplicationContext.Books.Add(book);
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/
        [HttpDelete]
        public IActionResult Delete()
        {
            ApplicationContext.Books.Clear();
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext.Books.Where(b => b.Id == id).FirstOrDefault();
            if(entity is null)
            {
                return NotFound();
            }
            else
            {
                ApplicationContext.Books.Remove(entity);
                return Ok();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
                return NotFound();

            bookPatch.ApplyTo(entity);
            return NoContent();
        }
    }
}

