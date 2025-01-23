using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class AgencyUser:BaseEntity
    {
        [Key]
        public Ulid IdAgencyUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public Boolean IsBlocked { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsFirstLogin { get; set; } = true;
        public string? Email { get; set; }
        public int? LoginCount { get; set; } = 0;
        public string Login { get; set; }
        public string Password { get; set; }
        public string Profil { get; set; }
        public string Address { get; set; }
        public Ulid FkIdStaff { get; set; }
        public OwnerAgent? Staff { get; set; }
        public Ulid FkIdAgency { get; set; }
        public Agency? Agency { get; set; }
        [JsonIgnore]
        public ICollection<InvoiceStartupMaster>? InvoiceStartupMasters { get; set; }
        [JsonIgnore]
        public ICollection<CustomerWallet>? CustomerWallets { get; set; }

    }
}
