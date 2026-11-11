using Microsoft.AspNetCore.Identity;
using System.Data;

namespace HelpDeskWeb.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
