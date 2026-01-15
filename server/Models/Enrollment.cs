using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSqliteApp.Models;

public class Enrollment
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    [ForeignKey("StudentId")]
    public virtual Student? Student { get; set; } 

    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }
}