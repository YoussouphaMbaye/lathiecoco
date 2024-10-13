using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class Partener:BaseEntity
    {
        [Key]
        public Ulid IdPartener { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Boolean IsBlocked { get; set; }
        public Boolean IsActive { get; set; }
        public string Logo { get; set; }
        [JsonIgnore]
        public ICollection<BillerInvoice>? BillerInvoices { get; set; }
        public Ulid FkIdAccounting { get; set; }
        public Accounting? Accounting { get; set; }

    }
}
