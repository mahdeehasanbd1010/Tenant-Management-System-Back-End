using System.ComponentModel.DataAnnotations;

namespace Tenant_Management_System_Server.Models
{
    public class LoginModel
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set;}
    }
}
