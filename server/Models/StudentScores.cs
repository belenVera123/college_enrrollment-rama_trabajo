using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSqliteApp.Models;

public class StudentScore
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AssessmentId { get; set; }

    [Required]
    public int EnrollmentId { get; set; }

    [Required]
    public int PuntajeObtenido { get; set; }

    public string? Observaciones { get; set; }

    [ForeignKey("AssessmentId")]
    public virtual Assessment? Assessment { get; set; }

    [ForeignKey("EnrollmentId")]
    public virtual Enrollment? Enrollment { get; set; }
}