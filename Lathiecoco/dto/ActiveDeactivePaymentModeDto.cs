using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class ActiveDeactivePaymentModeDto
    {
        [Required]
        public Ulid FkIdStaff { get; set; }
        [Required]
        public Ulid IdPaymentMode { get; set; }
    }
}
