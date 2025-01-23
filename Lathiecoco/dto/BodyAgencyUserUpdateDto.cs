using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class BodyAgencyUserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Profil { get; set; } 
        public string Address { get; set; }
        public Ulid FkidAgency { get; set; }
        public Ulid FkidStaff { get; set; }
    }
}
