using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class Order : BaseModel
    {
        /// <summary>
        /// Customer name
        /// </summary>
        /// <remarks>
        /// For Billing Information
        /// </remarks>
        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        /// <remarks>
        /// For Billing Information
        /// </remarks>
        [Required]
        [MaxLength(60)]
        public string? Address { get; set; }

        /// <summary>
        /// For Yuno
        /// </summary>
        [MinLength(36)]
        [MaxLength(64)]
        public string? CheckoutSession { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public Guid ClientUserId { get; set; }

        [ForeignKey(nameof(ClientUserId))]
        public virtual ClientUser? ClientUser { get; set; }

        /// <remarks>
        /// For Billing Information
        /// </remarks>
        [Required]
        public Guid CountryId { get; set; }

        /// <remarks>
        /// For Billing Information
        /// </remarks>
        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled,
        Refunded,
        Failed,
        //Shipped, 
        //Delivered, 
    }
}
