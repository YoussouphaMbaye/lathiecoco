using Lathiecoco.dto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lathiecoco.models
{
    public class OwnerAgent:BaseEntity
    {
        [Key]
        public Ulid IdOwnerAgent { get; set; }
        public string CodeOwnerAgent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public Boolean IsBlocked { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsFirstLogin { get; set; }=true;
        public string? Email { get; set; } 
        public int? LoginCount { get; set; } = 0;
        public string Login { get; set; }
        public string Password { get; set; }
        public string? TokenRefresh { get; set; }
        public DateTime? ExpireDateTokenRefresh { get; set; }
        public string Profil { get; set; }
        public string Address { get; set; }
        public string AgentType { get; set; }
       
        [JsonIgnore]
        public ICollection<InvoiceStartupMaster>? InvoiceStartupMasters { get; set; }
        [JsonIgnore]
        public ICollection<CustomerWallet>? CustomerWallets { get; set; }
        [JsonIgnore]
        public ICollection<PaymentMode>? PaymentModes { get; set; }
        [JsonIgnore]
        public ICollection<FeeSend>? FeeSends { get; set; }
        [JsonIgnore]
        public ICollection<Agency>? Agencies { get; set; }
        [JsonIgnore]
        public ICollection<UserLog>? UserLogs { get; set; }
        [JsonIgnore]
        public ICollection<AgencyUser>? AgencyUsers { get; set; }

    }
}
