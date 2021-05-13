using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Entity.RequestModels.User
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
