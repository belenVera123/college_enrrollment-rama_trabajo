using System;

namespace WebSqliteApp.Models;

public class AssessmentDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Puntaje { get; set; }
    public DateTime FechaEvaluacion { get; set; }
    public string? Descripcion { get; set; }
}