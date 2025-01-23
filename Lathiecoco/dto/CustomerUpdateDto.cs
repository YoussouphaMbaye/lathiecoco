using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class CustomerUpdateDto
    {
        [Required]
        public Ulid Id { get; set; }
        [Required]
        public Ulid FkIdStaff { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string Profile { get; set; }
        [Required]
        public string Address { get; set; }
        

    }
}
