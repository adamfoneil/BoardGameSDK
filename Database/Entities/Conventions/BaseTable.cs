using System.ComponentModel.DataAnnotations;

namespace Database.Entities.Conventions;

public class BaseTable
{
	public int Id { get; set; }

	[MaxLength(50)]
	public string CreatedBy { get; set; } = "system";
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	[MaxLength(50)]
	public string? ModifiedBy { get; set; }
	public DateTime? ModifiedAt { get; set; }
}
