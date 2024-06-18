using System.ComponentModel.DataAnnotations;

namespace Tusur.Practices.Server.Models.Request
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
