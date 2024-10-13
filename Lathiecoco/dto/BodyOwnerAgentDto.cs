using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.dto
{
    public class BodyOwnerAgentDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; } = "";
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(4,ErrorMessage ="Login must be 4 characters")]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Profil { get; set; }
        public string Address { get; set; }
        public string AgentType { get; set; } = "";


    }
}
