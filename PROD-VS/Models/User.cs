using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PROD_VS.Models
{
    public class User : IdentityUser
    {
        public string AccessLevel { get; set; } = "User"; // e.g., "Руководитель", "Инженер", etc.
        public bool IsDeveloper { get; set; } = false; // New flag for developer

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}

