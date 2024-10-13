using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class CustomerUpadatePinDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string NewPinNumber { get; set; }
        [Required]
        public string PhoneCountryIdentity { get; set; }
        [Required]
        public string OldPinNumber { get; set; }
    }
}
