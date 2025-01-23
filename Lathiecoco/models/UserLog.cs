using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Lathiecoco.models
{
    public class UserLog:BaseEntity
    {
        [Key]
        public Ulid IdUserLog { get; set; }
        public Ulid FkIdStaff { get; set; }
        public OwnerAgent? Staff { get; set; }
        public string UserAction {  get; set; }
        public string Code{  get; set; }
        public string IPaddress {  get; set; }
    }
}
