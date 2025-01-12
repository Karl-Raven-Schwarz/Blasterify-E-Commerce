using System.ComponentModel.DataAnnotations;

namespace Blasterify.Services.Models
{
    public class Country : BaseModel
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string? Code { get; set; }
    }
}