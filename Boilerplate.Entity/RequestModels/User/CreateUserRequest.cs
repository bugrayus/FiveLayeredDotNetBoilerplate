using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Entity.RequestModels.User
{
    public class CreateUserRequest
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is required.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
    }
}