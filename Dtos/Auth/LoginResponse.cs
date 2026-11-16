using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Dtos.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
