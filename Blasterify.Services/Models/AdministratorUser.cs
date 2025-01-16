﻿using System.ComponentModel.DataAnnotations;
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
        public int LogInAttempts { get; set; } = 0;

        /// <value>value="True": User Blocked</value>
        [Required]
        public bool IsLocked { get; set; } = false;

        [Required]
        public DateTime UnlockDate { get; set; }

        /// <remarks>Default Value = 10000000</remarks>
        [Range(10000000, 99999999)]
        public int VerificationCode { get; set; } = 10000000;

        public bool IsVerified { get; set; } = false;

        public DateTime VerificationCodeExpiration { get; set; } = DateTime.MinValue;

        [Required]
        public int ResetPasswordAttempts { get; set; } = 0;

        [Required]
        public Guid CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }
    }
}