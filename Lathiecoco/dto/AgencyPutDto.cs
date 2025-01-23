using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class AgencyPutDto
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        [Required]
        public Ulid fkIdStaff { get; set; }
    }
}
