using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blasterify.Services.Models
{
    public class MovieGenre : BaseModel
    {
        [Required]
        public Guid MovieId { get; set; }
        
        [ForeignKey(nameof(MovieId))]
        public virtual Movie? Movie { get; set; }

        [Required]
        public Guid GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre? Genre { get; set; }
    }
}