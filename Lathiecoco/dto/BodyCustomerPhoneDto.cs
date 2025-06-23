using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class BodyCustomerPhoneDto
    {
        [MinLength(9)]
        [MaxLength(9)]
        public string Phone { get; set; }
        public string CountryPhoneIdentity { get; set; }
        public string CountryCode { get; set; }
    }
}
