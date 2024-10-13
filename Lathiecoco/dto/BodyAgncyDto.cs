namespace Lathiecoco.dto
{
    public class BodyAgncyDto
    {
        public string Name { get; set; }
        public string code { get; set; }
        public string city { get; set; }
        public string adress { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Ulid FkIdCompagny { get; set; }

    }
}
