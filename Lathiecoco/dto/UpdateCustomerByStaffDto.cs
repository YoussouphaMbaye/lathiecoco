using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class UpdateCustomerByStaffDto
    {
        [Required]
        public Ulid IdStaff {  get; set; }

        [Required]
        public String PhoneNumber { get; set; }

        [Required]
        public String PhoneIdentity { get; set; }
    }
}
