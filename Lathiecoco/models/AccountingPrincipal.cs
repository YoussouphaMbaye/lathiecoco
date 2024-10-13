using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class AccountingPrincipal:BaseEntity
    {
        [Key]
        public Ulid IdAccountingPrincipal { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
       
        
        [JsonIgnore]
        public ICollection<AccountingOpPrincipal>? AccountingOpPrincipals { get; set; }

    }
}
