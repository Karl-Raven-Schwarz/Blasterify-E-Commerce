using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class Movie : BaseModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// Minutes
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public double Duration { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime PremiereDate { get; set; }

        /// <summary>
        /// 1.00 - 5.00
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        public double Rate { get; set; }

        [Required]
        public string? FirebasePosterId { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")]
        public double PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")]
        public double RentalPrice { get; set; }

        [Required]
        public bool IsFree { get; set; }

        // Add video url or id
    }
}