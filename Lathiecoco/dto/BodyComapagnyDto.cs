namespace Lathiecoco.dto
{
    public class BodyComapagnyDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Ulid? FkIdCountry { get; set; }
    }
}
