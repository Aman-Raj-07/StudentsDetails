using AAPI.Data;
using AAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StudController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _db.students.ToList();
            if (products.Count == 0)
            {
                return NotFound("No data found");
            }
            return Ok(products);
        }


        [HttpGet("{id=int}")]
        public IActionResult Get(int id)
        {
            var product = _db.students.Find(id);
            if (product == null)
            {
                return NotFound("No Student available");
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            _db.students.Add(student);
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Student student)
        {
            _db.students.Update(student);
            _db.SaveChanges();
            return Ok("Data updated");
        }
        [HttpDelete("{id=int}")]
        public IActionResult Delete(int id)
        {
            var product = _db.students.Find(id);
            if (product == null)
            {
                return NotFound("No Student available");
            }
            _db.students.Remove(product);
            _db.SaveChanges();
            return Ok("Data deleted");
        }

    }
}
