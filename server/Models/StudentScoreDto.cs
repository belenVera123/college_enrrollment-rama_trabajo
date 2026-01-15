using System;

namespace WebSqliteApp.Models;

public class StudentScoreDto
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int EnrollmentId { get; set; }
    public decimal PuntajeObtenido { get; set; }
    public string? Observaciones { get; set; }
    public string AssessmentName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
}