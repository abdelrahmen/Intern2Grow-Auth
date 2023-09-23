using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Intern2Grow_Auth.Models
{
    public class User:IdentityUser
    {
        
        [Required]
        [MaxLength(100)]
        public string Fullname { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
