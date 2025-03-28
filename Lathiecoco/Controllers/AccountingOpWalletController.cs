﻿using Lathiecoco.models;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingOpWalletController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private AccountingOpWalletRep _accountingOpRepService;
        public AccountingOpWalletController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            AccountingOpWalletRep accountingOpRepService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _accountingOpRepService = accountingOpRepService;
        }

        [Authorize]
        [HttpGet("/accounting-op-wallet/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWallet(int page = 1, int limit = 10)
        {

            return await _accountingOpRepService.findAllAccountingOpWallet(page, limit);

        }

        [Authorize]
        [HttpGet("/accounting-op-Wallet/with-accounting")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWalletWithAccounting(Ulid idAccounting,int page = 1, int limit = 10)
        {

            return await _accountingOpRepService.findAllAccountingOpWalletWithAccounting(idAccounting, page, limit );

        }

        [Authorize]
        [HttpGet("/accounting-op-Wallet/with-accounting/search")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<AccountingOpWallet>>> findAllAccountingOpWalletWithAccountingSearch(Ulid idAccounting, string? PaymentMethod, DateTime? beginDate, DateTime? endDate, int page=1, int limit=10)
        {

            return await _accountingOpRepService.searcheInvoiceWalletAgent(idAccounting, PaymentMethod, beginDate, endDate, page, limit);

        }
    }
}
