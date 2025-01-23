using Lathiecoco.models;

namespace Lathiecoco.dto
{
    public class AddUserLogDto
    {
        public Ulid FkIdStaff { get; set; }
        public string UserAction { get; set; }
        public string Code { get; set; }
        public string IPaddress { get; set; }
    }
}
