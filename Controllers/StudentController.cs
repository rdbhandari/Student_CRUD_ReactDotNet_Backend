using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _dbContext;

        public StudentController(StudentContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _dbContext.Students.FromSql($"SELECT * FROM Students").ToListAsync();

            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _dbContext.Students.FromSql($"SELECT TOP 1 * FROM Students WHERE Id = {id} ").ToListAsync();
            if (student == null)
            {
                return NotFound();
            }
            if (student.Count == 0)
            {
                return NoContent();
            }

            return Ok(student);
        }
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return Created(nameof(AddStudent), student);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Student>> EditStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(student).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();

        }
        private bool TodoItemExists(int id)
        {
            return (_dbContext.Students?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
