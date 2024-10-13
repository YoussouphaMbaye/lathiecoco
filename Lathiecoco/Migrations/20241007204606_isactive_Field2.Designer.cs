﻿// <auto-generated />
using System;
using Lathiecoco.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lathiecoco.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    [Migration("20241007204606_isactive_Field2")]
    partial class isactive_Field2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Lathiecoco.models.Accounting", b =>
                {
                    b.Property<string>("IdAccounting")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdAccounting");

                    b.ToTable("Accountings");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingOpPrincipal", b =>
                {
                    b.Property<string>("IdAccountingOpPrincipal")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Credited")
                        .HasColumnType("float");

                    b.Property<double>("DeBited")
                        .HasColumnType("float");

                    b.Property<string>("FkIdAccounting")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdInvoiceStartupMaster")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("NewBalance")
                        .HasColumnType("float");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdAccountingOpPrincipal");

                    b.HasIndex("FkIdAccounting");

                    b.HasIndex("FkIdInvoiceStartupMaster");

                    b.ToTable("AccountingOpPrincipals");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingOpWallet", b =>
                {
                    b.Property<string>("IdAccountingOperation")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Credited")
                        .HasColumnType("float");

                    b.Property<double>("DeBited")
                        .HasColumnType("float");

                    b.Property<string>("FkIdAccounting")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdBillerInvoice")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdInvoice")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdInvoiceStartupMaster")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdInvoiceWalletAgent")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("NewBalance")
                        .HasColumnType("float");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdAccountingOperation");

                    b.HasIndex("FkIdAccounting");

                    b.HasIndex("FkIdBillerInvoice");

                    b.HasIndex("FkIdInvoice");

                    b.HasIndex("FkIdInvoiceStartupMaster");

                    b.HasIndex("FkIdInvoiceWalletAgent");

                    b.ToTable("AccountingOpWallets");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingPrincipal", b =>
                {
                    b.Property<string>("IdAccountingPrincipal")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdAccountingPrincipal");

                    b.ToTable("AccountingPrincipals");
                });

            modelBuilder.Entity("Lathiecoco.models.BillerInvoice", b =>
                {
                    b.Property<string>("IdBillerInvoice")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountToPaid")
                        .HasColumnType("float");

                    b.Property<string>("BillerReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("FeesAmount")
                        .HasColumnType("float");

                    b.Property<string>("FkIdCustomerWallet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdFeeSend")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdPartener")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdPaymentMode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReloadBiller")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdBillerInvoice");

                    b.HasIndex("FkIdCustomerWallet");

                    b.HasIndex("FkIdFeeSend");

                    b.HasIndex("FkIdPartener");

                    b.HasIndex("FkIdPaymentMode");

                    b.HasIndex("InvoiceCode")
                        .IsUnique();

                    b.ToTable("BillerInvoices");
                });

            modelBuilder.Entity("Lathiecoco.models.CustomerWallet", b =>
                {
                    b.Property<string>("IdCustomerWallet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FkIdAccounting")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhoneBrand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinTemp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("phoneIdentity")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCustomerWallet");

                    b.HasIndex("FkIdAccounting")
                        .IsUnique();

                    b.HasIndex("phoneIdentity", "Phone")
                        .IsUnique();

                    b.ToTable("CustomerWallets");
                });

            modelBuilder.Entity("Lathiecoco.models.FeeSend", b =>
                {
                    b.Property<string>("IdFeeSend")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("FixeAgFee")
                        .HasColumnType("real");

                    b.Property<float>("FixeCsFee")
                        .HasColumnType("real");

                    b.Property<string>("FkIdPaymentMode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("MaxAmount")
                        .HasColumnType("float");

                    b.Property<double>("MinAmount")
                        .HasColumnType("float");

                    b.Property<float>("PercentAgFee")
                        .HasColumnType("real");

                    b.Property<float>("PercentCsFee")
                        .HasColumnType("real");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdFeeSend");

                    b.HasIndex("FkIdPaymentMode");

                    b.ToTable("FeeSends");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceStartupMaster", b =>
                {
                    b.Property<string>("IdInvoiceStartupMaster")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountToPaid")
                        .HasColumnType("float");

                    b.Property<double>("AmountToReceived")
                        .HasColumnType("float");

                    b.Property<double>("AmountToSend")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FkIdAgent")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdPaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdStaff")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdInvoiceStartupMaster");

                    b.HasIndex("FkIdAgent");

                    b.HasIndex("FkIdPaymentMode");

                    b.HasIndex("FkIdStaff");

                    b.HasIndex("InvoiceCode")
                        .IsUnique();

                    b.ToTable("InvoiceStartupMasters");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWallet", b =>
                {
                    b.Property<string>("IdInvoiceWallet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountToPaid")
                        .HasColumnType("float");

                    b.Property<double>("AmountToSend")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("FeesAmount")
                        .HasColumnType("float");

                    b.Property<string>("FkIdFeeSend")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdPaymentMode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdRecipient")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdSender")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdInvoiceWallet");

                    b.HasIndex("FkIdFeeSend");

                    b.HasIndex("FkIdPaymentMode");

                    b.HasIndex("FkIdRecipient");

                    b.HasIndex("FkIdSender");

                    b.HasIndex("InvoiceCode")
                        .IsUnique();

                    b.ToTable("InvoiceWallets");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWalletAgent", b =>
                {
                    b.Property<string>("IdInvoiceWalletCashier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountToPaid")
                        .HasColumnType("float");

                    b.Property<double>("AmountToSend")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("FeesAmount")
                        .HasColumnType("float");

                    b.Property<string>("FkIdAgent")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdCustomerWallet")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdFeeSend")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FkIdPaymentMode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InvoiceCode2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdInvoiceWalletCashier");

                    b.HasIndex("FkIdAgent");

                    b.HasIndex("FkIdCustomerWallet");

                    b.HasIndex("FkIdFeeSend");

                    b.HasIndex("FkIdPaymentMode");

                    b.HasIndex("InvoiceCode")
                        .IsUnique();

                    b.ToTable("InvoiceWalletAgents");
                });

            modelBuilder.Entity("Lathiecoco.models.MarchandComissionDaly", b =>
                {
                    b.Property<string>("IdMarchandComission")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Comission")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MaxAmount")
                        .HasColumnType("float");

                    b.Property<double>("MinAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdMarchandComission");

                    b.ToTable("MarchandComissionDalys");
                });

            modelBuilder.Entity("Lathiecoco.models.OwnerAgent", b =>
                {
                    b.Property<string>("IdOwnerAgent")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeOwnerAgent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFirstLogin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LoginCount")
                        .HasColumnType("int");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdOwnerAgent");

                    b.ToTable("OwnerAgents");
                });

            modelBuilder.Entity("Lathiecoco.models.Partener", b =>
                {
                    b.Property<string>("IdPartener")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FkIdAccounting")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdPartener");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("FkIdAccounting")
                        .IsUnique();

                    b.ToTable("Parteners");
                });

            modelBuilder.Entity("Lathiecoco.models.PaymentMode", b =>
                {
                    b.Property<string>("IdPaymentMode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("IdPaymentMode");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PaymentModes");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingOpPrincipal", b =>
                {
                    b.HasOne("Lathiecoco.models.AccountingPrincipal", "Accounting")
                        .WithMany("AccountingOpPrincipals")
                        .HasForeignKey("FkIdAccounting")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.InvoiceStartupMaster", "InvoiceStartupMaster")
                        .WithMany("AccountingOpPrincipals")
                        .HasForeignKey("FkIdInvoiceStartupMaster");

                    b.Navigation("Accounting");

                    b.Navigation("InvoiceStartupMaster");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingOpWallet", b =>
                {
                    b.HasOne("Lathiecoco.models.Accounting", "Accounting")
                        .WithMany("AccountingOpWallets")
                        .HasForeignKey("FkIdAccounting")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.BillerInvoice", "BillerInvoice")
                        .WithMany("AccountingOp")
                        .HasForeignKey("FkIdBillerInvoice");

                    b.HasOne("Lathiecoco.models.InvoiceWallet", "InvoiceWallet")
                        .WithMany("AccountingOpWallets")
                        .HasForeignKey("FkIdInvoice");

                    b.HasOne("Lathiecoco.models.InvoiceStartupMaster", "InvoiceStartupMaster")
                        .WithMany("AccountingOpWallet")
                        .HasForeignKey("FkIdInvoiceStartupMaster");

                    b.HasOne("Lathiecoco.models.InvoiceWalletAgent", "InvoiceWalletAgent")
                        .WithMany("AccountingOpWallets")
                        .HasForeignKey("FkIdInvoiceWalletAgent");

                    b.Navigation("Accounting");

                    b.Navigation("BillerInvoice");

                    b.Navigation("InvoiceStartupMaster");

                    b.Navigation("InvoiceWallet");

                    b.Navigation("InvoiceWalletAgent");
                });

            modelBuilder.Entity("Lathiecoco.models.BillerInvoice", b =>
                {
                    b.HasOne("Lathiecoco.models.CustomerWallet", "CustomerWallet")
                        .WithMany("BillerInvoices")
                        .HasForeignKey("FkIdCustomerWallet");

                    b.HasOne("Lathiecoco.models.FeeSend", "FeeSend")
                        .WithMany("BillerInvoices")
                        .HasForeignKey("FkIdFeeSend");

                    b.HasOne("Lathiecoco.models.Partener", "Partener")
                        .WithMany("BillerInvoices")
                        .HasForeignKey("FkIdPartener");

                    b.HasOne("Lathiecoco.models.PaymentMode", "PaymentModeObj")
                        .WithMany("BillerInvoices")
                        .HasForeignKey("FkIdPaymentMode");

                    b.Navigation("CustomerWallet");

                    b.Navigation("FeeSend");

                    b.Navigation("Partener");

                    b.Navigation("PaymentModeObj");
                });

            modelBuilder.Entity("Lathiecoco.models.CustomerWallet", b =>
                {
                    b.HasOne("Lathiecoco.models.Accounting", "Accounting")
                        .WithOne("CustomerWallet")
                        .HasForeignKey("Lathiecoco.models.CustomerWallet", "FkIdAccounting")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounting");
                });

            modelBuilder.Entity("Lathiecoco.models.FeeSend", b =>
                {
                    b.HasOne("Lathiecoco.models.PaymentMode", "PaymentMode")
                        .WithMany("FeeSends")
                        .HasForeignKey("FkIdPaymentMode");

                    b.Navigation("PaymentMode");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceStartupMaster", b =>
                {
                    b.HasOne("Lathiecoco.models.CustomerWallet", "Agent")
                        .WithMany("InvoiceStartupMasters")
                        .HasForeignKey("FkIdAgent")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.PaymentMode", "PaymentModeObj")
                        .WithMany("InvoiceStartupMasters")
                        .HasForeignKey("FkIdPaymentMode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.OwnerAgent", "Staff")
                        .WithMany("InvoiceStartupMasters")
                        .HasForeignKey("FkIdStaff");

                    b.Navigation("Agent");

                    b.Navigation("PaymentModeObj");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWallet", b =>
                {
                    b.HasOne("Lathiecoco.models.FeeSend", "FeeSend")
                        .WithMany("InvoiceWallet")
                        .HasForeignKey("FkIdFeeSend");

                    b.HasOne("Lathiecoco.models.PaymentMode", "PaymentModeObj")
                        .WithMany("InvoiceWallets")
                        .HasForeignKey("FkIdPaymentMode");

                    b.HasOne("Lathiecoco.models.CustomerWallet", "CustomerRecipient")
                        .WithMany("InvoiceWalletRecipeients")
                        .HasForeignKey("FkIdRecipient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.CustomerWallet", "CustomerSender")
                        .WithMany("InvoiceWalletSenders")
                        .HasForeignKey("FkIdSender")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CustomerRecipient");

                    b.Navigation("CustomerSender");

                    b.Navigation("FeeSend");

                    b.Navigation("PaymentModeObj");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWalletAgent", b =>
                {
                    b.HasOne("Lathiecoco.models.CustomerWallet", "Agent")
                        .WithMany("InvoiceWalletAgentAgents")
                        .HasForeignKey("FkIdAgent")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.CustomerWallet", "CustomerWallet")
                        .WithMany("InvoiceWalletAgents")
                        .HasForeignKey("FkIdCustomerWallet")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Lathiecoco.models.FeeSend", "FeeSend")
                        .WithMany("InvoiceWalletAgents")
                        .HasForeignKey("FkIdFeeSend");

                    b.HasOne("Lathiecoco.models.PaymentMode", "PaymentModeObj")
                        .WithMany("InvoiceWalletAgents")
                        .HasForeignKey("FkIdPaymentMode");

                    b.Navigation("Agent");

                    b.Navigation("CustomerWallet");

                    b.Navigation("FeeSend");

                    b.Navigation("PaymentModeObj");
                });

            modelBuilder.Entity("Lathiecoco.models.Partener", b =>
                {
                    b.HasOne("Lathiecoco.models.Accounting", "Accounting")
                        .WithOne("Partener")
                        .HasForeignKey("Lathiecoco.models.Partener", "FkIdAccounting")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounting");
                });

            modelBuilder.Entity("Lathiecoco.models.Accounting", b =>
                {
                    b.Navigation("AccountingOpWallets");

                    b.Navigation("CustomerWallet");

                    b.Navigation("Partener");
                });

            modelBuilder.Entity("Lathiecoco.models.AccountingPrincipal", b =>
                {
                    b.Navigation("AccountingOpPrincipals");
                });

            modelBuilder.Entity("Lathiecoco.models.BillerInvoice", b =>
                {
                    b.Navigation("AccountingOp");
                });

            modelBuilder.Entity("Lathiecoco.models.CustomerWallet", b =>
                {
                    b.Navigation("BillerInvoices");

                    b.Navigation("InvoiceStartupMasters");

                    b.Navigation("InvoiceWalletAgentAgents");

                    b.Navigation("InvoiceWalletAgents");

                    b.Navigation("InvoiceWalletRecipeients");

                    b.Navigation("InvoiceWalletSenders");
                });

            modelBuilder.Entity("Lathiecoco.models.FeeSend", b =>
                {
                    b.Navigation("BillerInvoices");

                    b.Navigation("InvoiceWallet");

                    b.Navigation("InvoiceWalletAgents");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceStartupMaster", b =>
                {
                    b.Navigation("AccountingOpPrincipals");

                    b.Navigation("AccountingOpWallet");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWallet", b =>
                {
                    b.Navigation("AccountingOpWallets");
                });

            modelBuilder.Entity("Lathiecoco.models.InvoiceWalletAgent", b =>
                {
                    b.Navigation("AccountingOpWallets");
                });

            modelBuilder.Entity("Lathiecoco.models.OwnerAgent", b =>
                {
                    b.Navigation("InvoiceStartupMasters");
                });

            modelBuilder.Entity("Lathiecoco.models.Partener", b =>
                {
                    b.Navigation("BillerInvoices");
                });

            modelBuilder.Entity("Lathiecoco.models.PaymentMode", b =>
                {
                    b.Navigation("BillerInvoices");

                    b.Navigation("FeeSends");

                    b.Navigation("InvoiceStartupMasters");

                    b.Navigation("InvoiceWalletAgents");

                    b.Navigation("InvoiceWallets");
                });
#pragma warning restore 612, 618
        }
    }
}
