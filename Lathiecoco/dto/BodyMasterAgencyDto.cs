using Lathiecoco.models;

namespace Lathiecoco.dto
{
    public class BodyMasterAgencyDto
    {
        //public Ulid IdMasterAgency { get; set; }
        public string Name { get; set; }
        public string code { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string adress { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Ulid FkIdAccountingMaster { get; set; }
    }
}
