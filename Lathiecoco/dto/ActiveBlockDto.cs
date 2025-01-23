using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class ActiveBlockDto
    {
        [Required]
        public Ulid FkIdStaff { get; set; }
        [Required]
        public Ulid IdUser { get; set; }
    }
}
