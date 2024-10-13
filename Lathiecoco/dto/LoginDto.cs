using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class LoginDto
    {
        [Required]
        [MinLength(4,ErrorMessage = "Username must be 4 characters")]
        public string username { get; set; }
        [MinLength(8, ErrorMessage = "Password must be 8 characters")]
        public string password { get; set; }
    }
}
