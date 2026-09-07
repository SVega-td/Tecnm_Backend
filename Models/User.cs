using Microsoft.AspNetCore.Identity;

namespace HelpDeskWeb.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
