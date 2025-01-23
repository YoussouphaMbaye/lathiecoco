using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class AgencyDto
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public Ulid fkIdStaff { get; set; }

    }
}
