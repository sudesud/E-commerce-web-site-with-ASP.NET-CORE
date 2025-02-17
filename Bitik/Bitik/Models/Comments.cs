using System;
using System.ComponentModel.DataAnnotations;

namespace Bitik.Models
{
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
