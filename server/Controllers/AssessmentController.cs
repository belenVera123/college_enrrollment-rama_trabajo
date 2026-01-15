using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models; 

namespace WebSqliteApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]

// [Authorize] 
public class AssessmentsController : ControllerBase
{
    private readonly AppDb _context;

    public AssessmentsController(AppDb context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssessmentDto>>> GetAssessments()
    {
        return await _context.Assessments
            .Include(a => a.Course)
            .Select(a => new AssessmentDto {
                Id = a.Id,
                CourseId = a.CourseId,
                CourseName = a.Course!.Nombre, 
                Nombre = a.Nombre,
                Puntaje = a.Puntaje,
                FechaEvaluacion = a.FechaEvaluacion,
                Descripcion = a.Descripcion
            }).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Assessment>> PostAssessment(Assessment assessment)
    {
        _context.Assessments.Add(assessment);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetAssessments), new { id = assessment.Id }, assessment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAssessment(int id, Assessment assessment)
    {
        if (id != assessment.Id) return BadRequest();

        _context.Entry(assessment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Assessments.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssessment(int id)
    {
        var assessment = await _context.Assessments.FindAsync(id);
        if (assessment == null) return NotFound();

        _context.Assessments.Remove(assessment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}