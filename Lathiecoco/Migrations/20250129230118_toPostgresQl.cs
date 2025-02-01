using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lathiecoco.Migrations
{
    /// <inheritdoc />
    public partial class toPostgresQl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingPrincipals",
                columns: table => new
                {
                    IdAccountingPrincipal = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingPrincipals", x => x.IdAccountingPrincipal);
                });

            migrationBuilder.CreateTable(
                name: "Accountings",
                columns: table => new
                {
                    IdAccounting = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountings", x => x.IdAccounting);
                });

            migrationBuilder.CreateTable(
                name: "MarchandComissionDalys",
                columns: table => new
                {
                    IdMarchandComission = table.Column<string>(type: "text", nullable: false),
                    MinAmount = table.Column<double>(type: "double precision", nullable: false),
                    MaxAmount = table.Column<double>(type: "double precision", nullable: false),
                    Comission = table.Column<double>(type: "double precision", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarchandComissionDalys", x => x.IdMarchandComission);
                });

            migrationBuilder.CreateTable(
                name: "OwnerAgents",
                columns: table => new
                {
                    IdOwnerAgent = table.Column<string>(type: "text", nullable: false),
                    CodeOwnerAgent = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    LoginCount = table.Column<int>(type: "integer", nullable: true),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    TokenRefresh = table.Column<string>(type: "text", nullable: true),
                    ExpireDateTokenRefresh = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Profil = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    AgentType = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerAgents", x => x.IdOwnerAgent);
                });

            migrationBuilder.CreateTable(
                name: "Parteners",
                columns: table => new
                {
                    IdPartener = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    FkIdAccounting = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parteners", x => x.IdPartener);
                    table.ForeignKey(
                        name: "FK_Parteners_Accountings_FkIdAccounting",
                        column: x => x.FkIdAccounting,
                        principalTable: "Accountings",
                        principalColumn: "IdAccounting",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    IdAgency = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    FkIdStaff = table.Column<string>(type: "text", nullable: false),
                    PercentagePurchase = table.Column<float>(type: "real", nullable: true),
                    FkIdAccounting = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.IdAgency);
                    table.ForeignKey(
                        name: "FK_Agencies_Accountings_FkIdAccounting",
                        column: x => x.FkIdAccounting,
                        principalTable: "Accountings",
                        principalColumn: "IdAccounting");
                    table.ForeignKey(
                        name: "FK_Agencies_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentModes",
                columns: table => new
                {
                    IdPaymentMode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    FkIdStaff = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentModes", x => x.IdPaymentMode);
                    table.ForeignKey(
                        name: "FK_PaymentModes_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent");
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    IdUserLog = table.Column<string>(type: "text", nullable: false),
                    FkIdStaff = table.Column<string>(type: "text", nullable: false),
                    UserAction = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    IPaddress = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.IdUserLog);
                    table.ForeignKey(
                        name: "FK_UserLogs_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgencyUsers",
                columns: table => new
                {
                    IdAgencyUser = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    LoginCount = table.Column<int>(type: "integer", nullable: true),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Profil = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    TokenRefresh = table.Column<string>(type: "text", nullable: true),
                    ExpireDateTokenRefresh = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FkIdStaff = table.Column<string>(type: "text", nullable: false),
                    FkIdAgency = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyUsers", x => x.IdAgencyUser);
                    table.ForeignKey(
                        name: "FK_AgencyUsers_Agencies_FkIdAgency",
                        column: x => x.FkIdAgency,
                        principalTable: "Agencies",
                        principalColumn: "IdAgency",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgencyUsers_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeSends",
                columns: table => new
                {
                    IdFeeSend = table.Column<string>(type: "text", nullable: false),
                    MinAmount = table.Column<double>(type: "double precision", nullable: false),
                    MaxAmount = table.Column<double>(type: "double precision", nullable: false),
                    FixeAgFee = table.Column<float>(type: "real", nullable: false),
                    FixeCsFee = table.Column<float>(type: "real", nullable: false),
                    PercentAgFee = table.Column<float>(type: "real", nullable: false),
                    PercentCsFee = table.Column<float>(type: "real", nullable: false),
                    FkIdPaymentMode = table.Column<string>(type: "text", nullable: true),
                    FkIdStaff = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeSends", x => x.IdFeeSend);
                    table.ForeignKey(
                        name: "FK_FeeSends_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent");
                    table.ForeignKey(
                        name: "FK_FeeSends_PaymentModes_FkIdPaymentMode",
                        column: x => x.FkIdPaymentMode,
                        principalTable: "PaymentModes",
                        principalColumn: "IdPaymentMode");
                });

            migrationBuilder.CreateTable(
                name: "CustomerWallets",
                columns: table => new
                {
                    IdCustomerWallet = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    phoneIdentity = table.Column<string>(type: "text", nullable: false),
                    PinNumber = table.Column<string>(type: "text", nullable: false),
                    PinTemp = table.Column<string>(type: "text", nullable: true),
                    Profile = table.Column<string>(type: "text", nullable: false),
                    PhoneBrand = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    FkIdStaff = table.Column<string>(type: "text", nullable: true),
                    FkIdAgencyUser = table.Column<string>(type: "text", nullable: true),
                    FkIdAccounting = table.Column<string>(type: "text", nullable: false),
                    PercentagePurchase = table.Column<float>(type: "real", nullable: true),
                    FkIdAgency = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerWallets", x => x.IdCustomerWallet);
                    table.ForeignKey(
                        name: "FK_CustomerWallets_Accountings_FkIdAccounting",
                        column: x => x.FkIdAccounting,
                        principalTable: "Accountings",
                        principalColumn: "IdAccounting",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerWallets_Agencies_FkIdAgency",
                        column: x => x.FkIdAgency,
                        principalTable: "Agencies",
                        principalColumn: "IdAgency",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerWallets_AgencyUsers_FkIdAgencyUser",
                        column: x => x.FkIdAgencyUser,
                        principalTable: "AgencyUsers",
                        principalColumn: "IdAgencyUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerWallets_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent");
                });

            migrationBuilder.CreateTable(
                name: "BillerInvoices",
                columns: table => new
                {
                    IdBillerInvoice = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode = table.Column<string>(type: "text", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "text", nullable: false),
                    BillerReference = table.Column<string>(type: "text", nullable: false),
                    ReloadBiller = table.Column<string>(type: "text", nullable: false),
                    AmountToPaid = table.Column<double>(type: "double precision", nullable: false),
                    FeesAmount = table.Column<double>(type: "double precision", nullable: false),
                    NumberOfKw = table.Column<double>(type: "double precision", nullable: true),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    FkIdCustomerWallet = table.Column<string>(type: "text", nullable: true),
                    FkIdPartener = table.Column<string>(type: "text", nullable: true),
                    FkIdPaymentMode = table.Column<string>(type: "text", nullable: true),
                    FkIdFeeSend = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillerInvoices", x => x.IdBillerInvoice);
                    table.ForeignKey(
                        name: "FK_BillerInvoices_CustomerWallets_FkIdCustomerWallet",
                        column: x => x.FkIdCustomerWallet,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet");
                    table.ForeignKey(
                        name: "FK_BillerInvoices_FeeSends_FkIdFeeSend",
                        column: x => x.FkIdFeeSend,
                        principalTable: "FeeSends",
                        principalColumn: "IdFeeSend");
                    table.ForeignKey(
                        name: "FK_BillerInvoices_Parteners_FkIdPartener",
                        column: x => x.FkIdPartener,
                        principalTable: "Parteners",
                        principalColumn: "IdPartener");
                    table.ForeignKey(
                        name: "FK_BillerInvoices_PaymentModes_FkIdPaymentMode",
                        column: x => x.FkIdPaymentMode,
                        principalTable: "PaymentModes",
                        principalColumn: "IdPaymentMode");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceStartupMasters",
                columns: table => new
                {
                    IdInvoiceStartupMaster = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode2 = table.Column<string>(type: "text", nullable: true),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    IsMaster = table.Column<bool>(type: "boolean", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "text", nullable: false),
                    ProofLink = table.Column<string>(type: "text", nullable: true),
                    AmountToSend = table.Column<double>(type: "double precision", nullable: false),
                    AmountToPaid = table.Column<double>(type: "double precision", nullable: false),
                    FkIdPaymentMode = table.Column<string>(type: "text", nullable: false),
                    ValidateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FkIdStaff = table.Column<string>(type: "text", nullable: true),
                    FkIdAgent = table.Column<string>(type: "text", nullable: true),
                    FkIdAgencyUser = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceStartupMasters", x => x.IdInvoiceStartupMaster);
                    table.ForeignKey(
                        name: "FK_InvoiceStartupMasters_AgencyUsers_FkIdAgencyUser",
                        column: x => x.FkIdAgencyUser,
                        principalTable: "AgencyUsers",
                        principalColumn: "IdAgencyUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceStartupMasters_CustomerWallets_FkIdAgent",
                        column: x => x.FkIdAgent,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet");
                    table.ForeignKey(
                        name: "FK_InvoiceStartupMasters_OwnerAgents_FkIdStaff",
                        column: x => x.FkIdStaff,
                        principalTable: "OwnerAgents",
                        principalColumn: "IdOwnerAgent");
                    table.ForeignKey(
                        name: "FK_InvoiceStartupMasters_PaymentModes_FkIdPaymentMode",
                        column: x => x.FkIdPaymentMode,
                        principalTable: "PaymentModes",
                        principalColumn: "IdPaymentMode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceWalletAgents",
                columns: table => new
                {
                    IdInvoiceWalletCashier = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode2 = table.Column<string>(type: "text", nullable: true),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    FkIdAgent = table.Column<string>(type: "text", nullable: false),
                    FkIdCustomerWallet = table.Column<string>(type: "text", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "text", nullable: false),
                    AmountToSend = table.Column<double>(type: "double precision", nullable: false),
                    AmountToPaid = table.Column<double>(type: "double precision", nullable: false),
                    FeesAmount = table.Column<double>(type: "double precision", nullable: false),
                    FkIdFeeSend = table.Column<string>(type: "text", nullable: true),
                    FkIdPaymentMode = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceWalletAgents", x => x.IdInvoiceWalletCashier);
                    table.ForeignKey(
                        name: "FK_InvoiceWalletAgents_CustomerWallets_FkIdAgent",
                        column: x => x.FkIdAgent,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceWalletAgents_CustomerWallets_FkIdCustomerWallet",
                        column: x => x.FkIdCustomerWallet,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceWalletAgents_FeeSends_FkIdFeeSend",
                        column: x => x.FkIdFeeSend,
                        principalTable: "FeeSends",
                        principalColumn: "IdFeeSend");
                    table.ForeignKey(
                        name: "FK_InvoiceWalletAgents_PaymentModes_FkIdPaymentMode",
                        column: x => x.FkIdPaymentMode,
                        principalTable: "PaymentModes",
                        principalColumn: "IdPaymentMode");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceWallets",
                columns: table => new
                {
                    IdInvoiceWallet = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode = table.Column<string>(type: "text", nullable: false),
                    InvoiceCode2 = table.Column<string>(type: "text", nullable: false),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    FkIdSender = table.Column<string>(type: "text", nullable: false),
                    FkIdRecipient = table.Column<string>(type: "text", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "text", nullable: false),
                    AmountToSend = table.Column<double>(type: "double precision", nullable: false),
                    AmountToPaid = table.Column<double>(type: "double precision", nullable: false),
                    FeesAmount = table.Column<double>(type: "double precision", nullable: false),
                    FkIdFeeSend = table.Column<string>(type: "text", nullable: true),
                    FkIdPaymentMode = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceWallets", x => x.IdInvoiceWallet);
                    table.ForeignKey(
                        name: "FK_InvoiceWallets_CustomerWallets_FkIdRecipient",
                        column: x => x.FkIdRecipient,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceWallets_CustomerWallets_FkIdSender",
                        column: x => x.FkIdSender,
                        principalTable: "CustomerWallets",
                        principalColumn: "IdCustomerWallet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceWallets_FeeSends_FkIdFeeSend",
                        column: x => x.FkIdFeeSend,
                        principalTable: "FeeSends",
                        principalColumn: "IdFeeSend");
                    table.ForeignKey(
                        name: "FK_InvoiceWallets_PaymentModes_FkIdPaymentMode",
                        column: x => x.FkIdPaymentMode,
                        principalTable: "PaymentModes",
                        principalColumn: "IdPaymentMode");
                });

            migrationBuilder.CreateTable(
                name: "AccountingOpPrincipals",
                columns: table => new
                {
                    IdAccountingOpPrincipal = table.Column<string>(type: "text", nullable: false),
                    Credited = table.Column<double>(type: "double precision", nullable: false),
                    DeBited = table.Column<double>(type: "double precision", nullable: false),
                    NewBalance = table.Column<double>(type: "double precision", nullable: false),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    FkIdAccounting = table.Column<string>(type: "text", nullable: false),
                    FkIdInvoiceStartupMaster = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingOpPrincipals", x => x.IdAccountingOpPrincipal);
                    table.ForeignKey(
                        name: "FK_AccountingOpPrincipals_AccountingPrincipals_FkIdAccounting",
                        column: x => x.FkIdAccounting,
                        principalTable: "AccountingPrincipals",
                        principalColumn: "IdAccountingPrincipal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountingOpPrincipals_InvoiceStartupMasters_FkIdInvoiceSta~",
                        column: x => x.FkIdInvoiceStartupMaster,
                        principalTable: "InvoiceStartupMasters",
                        principalColumn: "IdInvoiceStartupMaster");
                });

            migrationBuilder.CreateTable(
                name: "AccountingOpWallets",
                columns: table => new
                {
                    IdAccountingOperation = table.Column<string>(type: "text", nullable: false),
                    Credited = table.Column<double>(type: "double precision", nullable: false),
                    DeBited = table.Column<double>(type: "double precision", nullable: false),
                    NewBalance = table.Column<double>(type: "double precision", nullable: false),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    FkIdAccounting = table.Column<string>(type: "text", nullable: false),
                    FkIdInvoice = table.Column<string>(type: "text", nullable: true),
                    FkIdBillerInvoice = table.Column<string>(type: "text", nullable: true),
                    FkIdInvoiceWalletAgent = table.Column<string>(type: "text", nullable: true),
                    FkIdInvoiceStartupMaster = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingOpWallets", x => x.IdAccountingOperation);
                    table.ForeignKey(
                        name: "FK_AccountingOpWallets_Accountings_FkIdAccounting",
                        column: x => x.FkIdAccounting,
                        principalTable: "Accountings",
                        principalColumn: "IdAccounting",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountingOpWallets_BillerInvoices_FkIdBillerInvoice",
                        column: x => x.FkIdBillerInvoice,
                        principalTable: "BillerInvoices",
                        principalColumn: "IdBillerInvoice");
                    table.ForeignKey(
                        name: "FK_AccountingOpWallets_InvoiceStartupMasters_FkIdInvoiceStartu~",
                        column: x => x.FkIdInvoiceStartupMaster,
                        principalTable: "InvoiceStartupMasters",
                        principalColumn: "IdInvoiceStartupMaster");
                    table.ForeignKey(
                        name: "FK_AccountingOpWallets_InvoiceWalletAgents_FkIdInvoiceWalletAg~",
                        column: x => x.FkIdInvoiceWalletAgent,
                        principalTable: "InvoiceWalletAgents",
                        principalColumn: "IdInvoiceWalletCashier");
                    table.ForeignKey(
                        name: "FK_AccountingOpWallets_InvoiceWallets_FkIdInvoice",
                        column: x => x.FkIdInvoice,
                        principalTable: "InvoiceWallets",
                        principalColumn: "IdInvoiceWallet");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpPrincipals_FkIdAccounting",
                table: "AccountingOpPrincipals",
                column: "FkIdAccounting");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpPrincipals_FkIdInvoiceStartupMaster",
                table: "AccountingOpPrincipals",
                column: "FkIdInvoiceStartupMaster");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallets_FkIdAccounting",
                table: "AccountingOpWallets",
                column: "FkIdAccounting");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallets_FkIdBillerInvoice",
                table: "AccountingOpWallets",
                column: "FkIdBillerInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallets_FkIdInvoice",
                table: "AccountingOpWallets",
                column: "FkIdInvoice");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallets_FkIdInvoiceStartupMaster",
                table: "AccountingOpWallets",
                column: "FkIdInvoiceStartupMaster");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingOpWallets_FkIdInvoiceWalletAgent",
                table: "AccountingOpWallets",
                column: "FkIdInvoiceWalletAgent");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_FkIdAccounting",
                table: "Agencies",
                column: "FkIdAccounting",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_FkIdStaff",
                table: "Agencies",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_phone",
                table: "Agencies",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUsers_FkIdAgency",
                table: "AgencyUsers",
                column: "FkIdAgency");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUsers_FkIdStaff",
                table: "AgencyUsers",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoices_FkIdCustomerWallet",
                table: "BillerInvoices",
                column: "FkIdCustomerWallet");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoices_FkIdFeeSend",
                table: "BillerInvoices",
                column: "FkIdFeeSend");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoices_FkIdPartener",
                table: "BillerInvoices",
                column: "FkIdPartener");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoices_FkIdPaymentMode",
                table: "BillerInvoices",
                column: "FkIdPaymentMode");

            migrationBuilder.CreateIndex(
                name: "IX_BillerInvoices_InvoiceCode",
                table: "BillerInvoices",
                column: "InvoiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdAccounting",
                table: "CustomerWallets",
                column: "FkIdAccounting",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdAgency",
                table: "CustomerWallets",
                column: "FkIdAgency");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdAgencyUser",
                table: "CustomerWallets",
                column: "FkIdAgencyUser");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_FkIdStaff",
                table: "CustomerWallets",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_phoneIdentity_Phone",
                table: "CustomerWallets",
                columns: new[] { "phoneIdentity", "Phone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeSends_FkIdPaymentMode",
                table: "FeeSends",
                column: "FkIdPaymentMode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeSends_FkIdStaff",
                table: "FeeSends",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_FkIdAgencyUser",
                table: "InvoiceStartupMasters",
                column: "FkIdAgencyUser");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_FkIdAgent",
                table: "InvoiceStartupMasters",
                column: "FkIdAgent");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_FkIdPaymentMode",
                table: "InvoiceStartupMasters",
                column: "FkIdPaymentMode");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_FkIdStaff",
                table: "InvoiceStartupMasters",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceStartupMasters_InvoiceCode",
                table: "InvoiceStartupMasters",
                column: "InvoiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgents_FkIdAgent",
                table: "InvoiceWalletAgents",
                column: "FkIdAgent");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgents_FkIdCustomerWallet",
                table: "InvoiceWalletAgents",
                column: "FkIdCustomerWallet");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgents_FkIdFeeSend",
                table: "InvoiceWalletAgents",
                column: "FkIdFeeSend");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgents_FkIdPaymentMode",
                table: "InvoiceWalletAgents",
                column: "FkIdPaymentMode");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWalletAgents_InvoiceCode",
                table: "InvoiceWalletAgents",
                column: "InvoiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWallets_FkIdFeeSend",
                table: "InvoiceWallets",
                column: "FkIdFeeSend");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWallets_FkIdPaymentMode",
                table: "InvoiceWallets",
                column: "FkIdPaymentMode");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWallets_FkIdRecipient",
                table: "InvoiceWallets",
                column: "FkIdRecipient");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWallets_FkIdSender",
                table: "InvoiceWallets",
                column: "FkIdSender");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWallets_InvoiceCode",
                table: "InvoiceWallets",
                column: "InvoiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parteners_Code",
                table: "Parteners",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parteners_FkIdAccounting",
                table: "Parteners",
                column: "FkIdAccounting",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentModes_FkIdStaff",
                table: "PaymentModes",
                column: "FkIdStaff");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentModes_Name",
                table: "PaymentModes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_FkIdStaff",
                table: "UserLogs",
                column: "FkIdStaff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingOpPrincipals");

            migrationBuilder.DropTable(
                name: "AccountingOpWallets");

            migrationBuilder.DropTable(
                name: "MarchandComissionDalys");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "AccountingPrincipals");

            migrationBuilder.DropTable(
                name: "BillerInvoices");

            migrationBuilder.DropTable(
                name: "InvoiceStartupMasters");

            migrationBuilder.DropTable(
                name: "InvoiceWalletAgents");

            migrationBuilder.DropTable(
                name: "InvoiceWallets");

            migrationBuilder.DropTable(
                name: "Parteners");

            migrationBuilder.DropTable(
                name: "CustomerWallets");

            migrationBuilder.DropTable(
                name: "FeeSends");

            migrationBuilder.DropTable(
                name: "AgencyUsers");

            migrationBuilder.DropTable(
                name: "PaymentModes");

            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "Accountings");

            migrationBuilder.DropTable(
                name: "OwnerAgents");
        }
    }
}
