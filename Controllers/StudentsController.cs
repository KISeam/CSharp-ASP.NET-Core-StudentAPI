using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudents(
            [FromQuery] int? id,
            [FromQuery] string? name,
            [FromQuery] double? cgpa,
            [FromQuery] string? address,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Students.Where(s => !s.IsDeleted).AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(s => s.Id == id.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            if (cgpa.HasValue)
            {
                query = query.Where(s => s.CGPA >= cgpa.Value);
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(s => s.Address.ToLower().Contains(address.ToLower().Trim()));
            }

            var totalRecords = await query.CountAsync();
            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                message = "Students fetched successfully.",
                pagination = new { totalRecords, pageNumber, pageSize },
                data = students
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetStudentById(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (student == null)
            {
                return NotFound(new { error = $"Student with ID {id} not found or has been deleted." });
            }

            return Ok(new
            {
                message = "Student details retrieved successfully.",
                data = student
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] Student newStudent)
        {
            if (newStudent == null)
            {
                return BadRequest(new { error = "Invalid student data." });
            }

            if (string.IsNullOrWhiteSpace(newStudent.Name) || string.IsNullOrWhiteSpace(newStudent.Email))
            {
                return BadRequest(new { error = "Name and Email are required fields." });
            }

            if (!EmailRegex.IsMatch(newStudent.Email))
            {
                return BadRequest(new { error = "The provided email address format is invalid." });
            }

            if (newStudent.CGPA < 0.0 || newStudent.CGPA > 4.0)
            {
                return BadRequest(new { error = "CGPA must be between 0.00 and 4.00." });
            }

            if (newStudent.Age < 18 || newStudent.Age > 60)
            {
                return BadRequest(new { error = "Age must be between 18 and 60 years." });
            }

            var idExists = await _context.Students.AnyAsync(s => s.Id == newStudent.Id);
            if (idExists && newStudent.Id != 0)
            {
                return Conflict(new { error = $"A student with ID {newStudent.Id} already exists." });
            }

            var emailExists = await _context.Students.AnyAsync(s => s.Email.ToLower() == newStudent.Email.ToLower().Trim());
            if (emailExists)
            {
                return Conflict(new { error = $"The email address '{newStudent.Email}' is already registered." });
            }

            newStudent.Name = newStudent.Name.Trim();
            newStudent.Email = newStudent.Email.ToLower().Trim();
            newStudent.Department = newStudent.Department.Trim();
            newStudent.Address = newStudent.Address.Trim();
            newStudent.Phone = newStudent.Phone.Trim();

            await _context.Students.AddAsync(newStudent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, new
            {
                message = "Student record created successfully.",
                data = newStudent
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return BadRequest(new { error = "Invalid student data." });
            }

            if (updatedStudent.Id != 0 && updatedStudent.Id != id)
            {
                return BadRequest(new { error = "Student ID mismatch. Altering the primary key ID is not allowed." });
            }

            if (string.IsNullOrWhiteSpace(updatedStudent.Name) || string.IsNullOrWhiteSpace(updatedStudent.Email))
            {
                return BadRequest(new { error = "Name and Email cannot be empty." });
            }

            if (!EmailRegex.IsMatch(updatedStudent.Email))
            {
                return BadRequest(new { error = "The provided email address format is invalid." });
            }

            if (updatedStudent.CGPA < 0.0 || updatedStudent.CGPA > 4.0)
            {
                return BadRequest(new { error = "CGPA must be between 0.00 and 4.00." });
            }

            if (updatedStudent.Age < 18 || updatedStudent.Age > 60)
            {
                return BadRequest(new { error = "Age must be between 18 and 60 years." });
            }

            var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (existingStudent == null)
            {
                return NotFound(new { error = $"Student with ID {id} not found." });
            }

            var emailExists = await _context.Students.AnyAsync(s => s.Email.ToLower() == updatedStudent.Email.ToLower().Trim() && s.Id != id);
            if (emailExists)
            {
                return Conflict(new { error = $"Cannot update student. The email '{updatedStudent.Email}' is already taken." });
            }

            existingStudent.Name = updatedStudent.Name.Trim();
            existingStudent.Email = updatedStudent.Email.ToLower().Trim();
            existingStudent.Department = updatedStudent.Department.Trim();
            existingStudent.CGPA = updatedStudent.CGPA;
            existingStudent.Phone = updatedStudent.Phone.Trim();
            existingStudent.Address = updatedStudent.Address.Trim();
            existingStudent.Age = updatedStudent.Age;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Student with ID {id} has been updated successfully.",
                data = existingStudent
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (student == null)
            {
                return NotFound(new { error = $"Student with ID {id} not found." });
            }

            student.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Student with ID {id} has been securely archived (Soft Deleted) successfully."
            });
        }
    }
}
