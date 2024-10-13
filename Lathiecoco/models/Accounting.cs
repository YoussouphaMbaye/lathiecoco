using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class Accounting:BaseEntity
    {
        [Key]
        public Ulid IdAccounting { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
        [JsonIgnore]
        public CustomerWallet? CustomerWallet { get; set; }
        [JsonIgnore]
        public Partener? Partener { get; set; }
        [JsonIgnore]
        public ICollection<AccountingOpWallet>? AccountingOpWallets { get; set; }
    }
}
