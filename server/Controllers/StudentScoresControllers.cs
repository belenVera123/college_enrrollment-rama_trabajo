using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models;

namespace WebSqliteApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class StudentScoresController : ControllerBase
{
    private readonly AppDb _context;

    public StudentScoresController(AppDb context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentScoreDto>>> GetStudentScores()
    {
        return await _context.StudentScores
            .Include(s => s.Assessment)
                .ThenInclude(a => a.Course)
            .Include(s => s.Enrollment)
                .ThenInclude(e => e.Student)
            .Select(s => new StudentScoreDto
            {
                Id = s.Id,
                AssessmentId = s.AssessmentId,
                EnrollmentId = s.EnrollmentId,
                PuntajeObtenido = s.PuntajeObtenido,
                Observaciones = s.Observaciones,

                AssessmentName = s.Assessment!.Nombre,
                CourseName = s.Assessment.Course!.Nombre,
                StudentName = s.Enrollment!.Student!.Nombre + " " + s.Enrollment.Student!.Apellido
            }).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentScoreDto>> GetById(int id)
    {
        var score = await _context.StudentScores
            .Include(s => s.Assessment)
                .ThenInclude(a => a.Course)
            .Include(s => s.Enrollment)
                .ThenInclude(e => e.Student)
            .Select(s => new StudentScoreDto
            {
                Id = s.Id,
                AssessmentId = s.AssessmentId,
                EnrollmentId = s.EnrollmentId,
                PuntajeObtenido = s.PuntajeObtenido,
                Observaciones = s.Observaciones,
                AssessmentName = s.Assessment!.Nombre,
                CourseName = s.Assessment.Course!.Nombre,
                StudentName = s.Enrollment!.Student!.Nombre + " " + s.Enrollment.Student!.Apellido
            })
            .FirstOrDefaultAsync(s => s.Id == id);

        if (score == null) return NotFound();

        return Ok(score);
    }

    [HttpPost]
    public async Task<ActionResult<StudentScore>> PostStudentScore(StudentScore studentScore)
    {
        _context.StudentScores.Add(studentScore);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { id = studentScore.Id }, studentScore);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudentScore(int id)
    {
        var score = await _context.StudentScores.FindAsync(id);
        if (score == null) return NotFound();

        _context.StudentScores.Remove(score);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Puntaje eliminado correctamente" });
    }
}