﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class PreRentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")]
        public double Price { get; set; }

        [Required]
        [ForeignKey("PreRent")]
        public Guid RentId { get; set; }

        [Required]
        public int RentDuration { get; set; }
    }
}