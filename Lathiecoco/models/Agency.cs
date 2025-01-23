
using Lathiecoco.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class Agency:BaseEntity
    {

        [Key]
        public Ulid IdAgency { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string code { get; set; }
        public bool isActive { get; set; } = true;
        public Ulid FkIdStaff { get; set; }
        public OwnerAgent? Staff { get; set; }
        public float? PercentagePurchase { get; set; }
        public Ulid? FkIdAccounting { get; set; }
        public Accounting? Accounting { get; set; }
        [JsonIgnore]
        public ICollection<AgencyUser>? AgencyUsers { get; set; }
        [JsonIgnore]
        public ICollection<CustomerWallet>? CustomerWallets { get; set; }

    }
}
