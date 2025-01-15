using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class AdministratorUser : BaseModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string? LastName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(35)]
        public string? Email { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public bool IsConnected { get; set; }

        [Required]
        public DateTime LastConnectionDate { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }
    }
}