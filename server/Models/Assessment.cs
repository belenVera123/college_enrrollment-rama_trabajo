using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebSqliteApp.Models;

public class Assessment {
    [Key]
    public int Id { get; set; }

    [Required]
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }  = string.Empty;

    [Required]
    [Range(0, 100)]
    public int Puntaje { get; set; }

    [Required]
    public DateTime FechaEvaluacion { get; set; }

    [StringLength(500)]
    public string? Descripcion { get; set; }
}