using System.ComponentModel.DataAnnotations;

namespace Lathiecoco.models
{
    public class MarchandComissionDaly:BaseEntity
    {
        [Key]
        public Ulid IdMarchandComission { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public double Comission {  get; set; }
    }
}
