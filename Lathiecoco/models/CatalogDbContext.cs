
using Microsoft.EntityFrameworkCore;

namespace Lathiecoco.models
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        
        public DbSet<CustomerWallet> CustomerWallets { get; set; }
        public DbSet<Accounting> Accountings { get; set; }
        public DbSet<FeeSend> FeeSends { get; set; }
        public DbSet<AccountingOpPrincipal> AccountingOpPrincipals { get; set; }
        public DbSet<InvoiceWallet> InvoiceWallets { get; set; }
        public DbSet<InvoiceWalletAgent> InvoiceWalletAgents { get; set; }
        public DbSet<AccountingOpWallet> AccountingOpWallets { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        
        public DbSet<AccountingPrincipal> AccountingPrincipals { get; set; }
        public DbSet<OwnerAgent> OwnerAgents { get; set; }
        public DbSet<InvoiceStartupMaster> InvoiceStartupMasters { get; set; }
        public DbSet<Partener> Parteners { get; set; }
        public DbSet<BillerInvoice> BillerInvoices { get; set; }
        public DbSet<MarchandComissionDaly> MarchandComissionDalys {  get; set; }


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Country>().Property(c => c.Id).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);

            //relations
            modelBuilder.Entity<Partener>().HasOne(c => c.Accounting).WithOne(a => a.Partener).HasForeignKey<Partener>(c => c.FkIdAccounting);

            modelBuilder.Entity<CustomerWallet>().HasOne(c => c.Accounting).WithOne(a => a.CustomerWallet).HasForeignKey<CustomerWallet>(c => c.FkIdAccounting);
            modelBuilder.Entity<Partener>().HasMany(c => c.BillerInvoices).WithOne(i => i.Partener).HasForeignKey(e => e.FkIdPartener);
            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.BillerInvoices).WithOne(i => i.CustomerWallet).HasForeignKey(e => e.FkIdCustomerWallet);


            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.InvoiceWalletSenders).WithOne(i => i.CustomerSender).HasForeignKey(e => e.FkIdSender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.InvoiceWalletRecipeients).WithOne(i => i.CustomerRecipient).HasForeignKey(e => e.FkIdRecipient).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.InvoiceWalletAgents).WithOne(i => i.CustomerWallet).HasForeignKey(e => e.FkIdCustomerWallet).OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Cashier>().HasMany(c => c.InvoiceWalletSenders).WithOne(i => i.CashierSender).HasForeignKey(e => e.FkIdCashierSender).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Cashier>().HasMany(c => c.InvoiceWalletPayees).WithOne(i => i.CashierPayee).HasForeignKey(e => e.FkIdCashierPayee).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.InvoiceWalletAgentAgents).WithOne(i => i.Agent).HasForeignKey(e => e.FkIdAgent).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomerWallet>().HasMany(c => c.InvoiceStartupMasters).WithOne(i => i.Agent).HasForeignKey(e => e.FkIdAgent);
            
            //modelBuilder.Entity<Agent>().HasMany(c => c.Maters).WithOne(i => i.Master).HasForeignKey(e => e.FkIdMaster).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Agent>().HasMany(c => c.Agents).WithOne(i => i.Cashier).HasForeignKey(e => e.FkIdCashier).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<InvoiceWallet>().HasMany(c => c.AccountingOpWallets).WithOne(i => i.InvoiceWallet).HasForeignKey(e => e.FkIdInvoice);
            modelBuilder.Entity<InvoiceWalletAgent>().HasMany(c => c.AccountingOpWallets).WithOne(i => i.InvoiceWalletAgent).HasForeignKey(e => e.FkIdInvoiceWalletAgent);
            modelBuilder.Entity<BillerInvoice>().HasMany(c => c.AccountingOp).WithOne(i => i.BillerInvoice).HasForeignKey(e => e.FkIdBillerInvoice);

            modelBuilder.Entity<Accounting>().HasMany(c => c.AccountingOpWallets).WithOne(c => c.Accounting).HasForeignKey(a => a.FkIdAccounting);

            modelBuilder.Entity<InvoiceWalletAgent>().HasMany(c => c.AccountingOpWallets).WithOne(c => c.InvoiceWalletAgent).HasForeignKey(a => a.FkIdInvoiceWalletAgent);
            modelBuilder.Entity<InvoiceStartupMaster>().HasMany(c => c.AccountingOpWallet).WithOne(c => c.InvoiceStartupMaster).HasForeignKey(a => a.FkIdInvoiceStartupMaster);
            modelBuilder.Entity<InvoiceStartupMaster>().HasMany(c => c.AccountingOpPrincipals).WithOne(c => c.InvoiceStartupMaster).HasForeignKey(a => a.FkIdInvoiceStartupMaster);
            modelBuilder.Entity<AccountingPrincipal>().HasMany(c => c.AccountingOpPrincipals).WithOne(c => c.Accounting).HasForeignKey(a => a.FkIdAccounting);


            //modelBuilder.Entity<Agency>().HasMany(c => c.FeeSends).WithOne(i => i.Agency).HasForeignKey(f => f.FkIdAgency);
            //modelBuilder.Entity<Agency>().HasMany(c => c.FeePayees).WithOne(i => i.Agency).HasForeignKey(e => e.FkIdAgency);

            modelBuilder.Entity<FeeSend>().HasMany(c => c.BillerInvoices).WithOne(i => i.FeeSend).HasForeignKey(e => e.FkIdFeeSend);

            modelBuilder.Entity<FeeSend>().HasMany(c => c.InvoiceWallet).WithOne(i => i.FeeSend).HasForeignKey(e => e.FkIdFeeSend);
            modelBuilder.Entity<FeeSend>().HasMany(c => c.InvoiceWalletAgents).WithOne(i => i.FeeSend).HasForeignKey(e => e.FkIdFeeSend);
            
            modelBuilder.Entity<OwnerAgent>().HasMany(c => c.InvoiceStartupMasters).WithOne(i => i.Staff).HasForeignKey(e => e.FkIdStaff);

            modelBuilder.Entity<PaymentMode>().HasMany(c => c.BillerInvoices).WithOne(i => i.PaymentModeObj).HasForeignKey(e => e.FkIdPaymentMode);

            modelBuilder.Entity<PaymentMode>().HasMany(c => c.InvoiceWallets).WithOne(i => i.PaymentModeObj).HasForeignKey(e => e.FkIdPaymentMode);
            modelBuilder.Entity<PaymentMode>().HasMany(c => c.InvoiceWalletAgents).WithOne(i => i.PaymentModeObj).HasForeignKey(e => e.FkIdPaymentMode);
            modelBuilder.Entity<PaymentMode>().HasMany(c => c.InvoiceStartupMasters).WithOne(i => i.PaymentModeObj).HasForeignKey(e => e.FkIdPaymentMode);
            modelBuilder.Entity<PaymentMode>().HasMany(c => c.FeeSends).WithOne(i => i.PaymentMode).HasForeignKey(e => e.FkIdPaymentMode);
            

            modelBuilder.Entity<CustomerWallet>().HasIndex(c => new { c.phoneIdentity, c.Phone }).IsUnique(true);

            modelBuilder.Entity<InvoiceWallet>().HasIndex(i => new { i.InvoiceCode }).IsUnique(true);
            modelBuilder.Entity<InvoiceWalletAgent>().HasIndex(i => new { i.InvoiceCode }).IsUnique(true);
            modelBuilder.Entity<InvoiceStartupMaster>().HasIndex(i => new { i.InvoiceCode }).IsUnique(true);

            //modelBuilder.Entity<Agent>().HasIndex(a => new { a.CodeAgent }).IsUnique(true);
            modelBuilder.Entity<Partener>().HasIndex(a => new { a.Code }).IsUnique(true);
            modelBuilder.Entity<BillerInvoice>().HasIndex(b => new { b.InvoiceCode }).IsUnique(true);
            //modelBuilder.Entity<FeeSend>().HasIndex(c => new {  c.FkIdCorridor,c.FkIdPaymentMode }).IsUnique(true);
            //modelBuilder.Entity<FeePayee>().HasIndex(c => new { c.FkIdPaymentMode, c.FkIdCorridor }).IsUnique(true);


            modelBuilder.Entity<PaymentMode>().HasIndex(c => new { c.Name }).IsUnique(true);

            modelBuilder.Entity<MarchandComissionDaly>().Property(m=>m.IdMarchandComission)
                .HasConversion(m=>m.ToString(),m=>Ulid.Parse(m));
            modelBuilder.Entity<Accounting>().Property(x => x.IdAccounting)
                .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<AccountingOpPrincipal>().Property(x => x.IdAccountingOpPrincipal)
                .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<AccountingOpWallet>().Property(x => x.IdAccountingOperation)
                .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<AccountingPrincipal>().Property(x => x.IdAccountingPrincipal)
                .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<BillerInvoice>().Property(x => x.IdBillerInvoice)
               .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<CustomerWallet>().Property(x => x.IdCustomerWallet)
               .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<FeeSend>().Property(x => x.IdFeeSend)
               .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<InvoiceStartupMaster>().Property(x => x.IdInvoiceStartupMaster)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<InvoiceWallet>().Property(x => x.IdInvoiceWallet)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<InvoiceWalletAgent>().Property(x => x.IdInvoiceWalletCashier)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<OwnerAgent>().Property(x => x.IdOwnerAgent)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<Partener>().Property(x => x.IdPartener)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            modelBuilder.Entity<PaymentMode>().Property(x => x.IdPaymentMode)
              .HasConversion(x => x.ToString(), x => Ulid.Parse(x));
            
            //modelBuilder.Entity<Supplier>().HasOne(c => c.buyProduct).WithOne(b => b.supplier).HasForeignKey<BuyProduct>(b => b.FkIdSupplier);

        }
    }
}
