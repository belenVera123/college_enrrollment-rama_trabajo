using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models;

namespace WebSqliteApp.Controllers;

[ApiController]
//[Authorize]
[Route("api/v1/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly AppDb _db;
    public EnrollmentsController(AppDb db) { _db = db; }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EnrollmentDto), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) 
    {
        try
        {
            var enrollment = _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    FechaInscripcion = e.FechaInscripcion,
                    StudentName = e.Student!.Nombre + " " + e.Student.Apellido,
                    CourseName = e.Course!.Nombre
                })
                .SingleOrDefault(x => x.Id == id);

            if (enrollment == null) return NotFound();
            
            return Ok(enrollment);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    public IActionResult GetAll([FromQuery] string? filtro, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            var query = _db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                query = query.Where(s =>
                    s.Student!.Nombre.Contains(filtro) || 
                    s.Course!.Nombre.Contains(filtro));
            }

            var total = query.Count();
            
            var items = query
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    FechaInscripcion = e.FechaInscripcion,
                    StudentName = e.Student!.Nombre + " " + e.Student.Apellido,
                    CourseName = e.Course!.Nombre
                })
                .OrderBy(s => s.StudentName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { total, page, pageSize, items });
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] EnrollmentDto dto)
    {
        var s = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            FechaInscripcion = DateTime.Now
        };
        
        _db.Enrollments.Add(s);
        try
        {
            _db.SaveChanges();
            return Ok(s);
        }
        catch
        {
            return BadRequest("Hubo un error al guardar los datos.");
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] EnrollmentDto dto)
    {
        var s = _db.Enrollments.Find(id);
        if (s is null) return NotFound();

        s.StudentId = dto.StudentId;
        s.CourseId = dto.CourseId;

        try
        {
            _db.SaveChanges();
            return Ok(s);
        }
        catch
        {
            return BadRequest("Hubo un error al intentar actualizar los datos");
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var s = _db.Enrollments.Find(id);
        if (s is null) return NotFound();

        try
        {
            _db.Remove(s);
            _db.SaveChanges();
            return Ok(new { ok = true });
        }
        catch
        {
            return BadRequest("Ocurrió algún error");
        }
    }
}