using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class ChangePasswordDto
    {
        [Required]
        public Ulid Id { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
