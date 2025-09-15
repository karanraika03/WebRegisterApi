using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiDomain.Model;

public class Blog
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime IsPublished { get; set; }
    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee Employee { get; set; }
}
