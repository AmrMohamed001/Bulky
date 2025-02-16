using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Range(1, 100, ErrorMessage = "Display Order for category must be greater than 0 and less than 100")]
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }
}