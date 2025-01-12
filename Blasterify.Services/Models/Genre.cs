using System.ComponentModel.DataAnnotations;

namespace Blasterify.Services.Models
{
    public class Genre : BaseModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}