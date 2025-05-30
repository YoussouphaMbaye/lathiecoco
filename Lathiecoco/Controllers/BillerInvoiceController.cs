﻿using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillerInvoiceController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IConfiguration _configuration;

        private BilllerInvoiceRep _billerInvoiceServ;
        public BillerInvoiceController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
           BilllerInvoiceRep billerInvoiceServ,
           IHttpContextAccessor contextAccessor)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;
            _contextAccessor = contextAccessor;

            _configuration = configuration;

            _billerInvoiceServ = billerInvoiceServ;
        }
        [HttpGet("/biller-invoice")]
        [Authorize]
        public async Task<ResponseBody<List<BillerInvoice>>> findAllbillerInvoice(int page = 1, int limit = 10)
        {
            //_contextAccessor.HttpContext.Response.Cookies.Append("token", "mtoken");
            return await _billerInvoiceServ.findAllBillerInvoice(page,limit);

        }
        [HttpGet("/biller-invoice/searche")]
        [Authorize]
        public async Task<ResponseBody<List<BillerInvoice>>> searcheBillerInvoice(string? idPaymentMode, string? code, DateTime? beginDate, DateTime? endDate,String? phone, String? billerReference, string? invoiceStatus, int page = 1, int limit = 10)

        {

            return await _billerInvoiceServ.searcheBillerInvoice(idPaymentMode, code, beginDate, endDate,phone, billerReference, invoiceStatus, page, limit )
;

        }

        [Authorize]
        [HttpPost("/biller-invoice")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ResponseBody<BillerInvoice>> addbillerInvoice(BodyBillerDto biller)
        {

            return await _billerInvoiceServ.insertBillerInvoice(biller);

        }

        [Authorize]
        [HttpGet("/biller-invoice/find-by-id")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<BillerInvoice>> billerById(Ulid id)
        {

            return await _billerInvoiceServ.findBillerInvoiceById(id);

        }
        [Authorize(Roles = "ADMIN,COMPTABLE,SUPADMIN")]
        [HttpGet("/biller-invoice/biller-by-agent-Sum-biller-amount")]
        public async Task<ActionResult> billerByAgentSumBiller(DateTime begenDate, DateTime endDate, Ulid? idAgent, Ulid? fkIdAgency)
        {

            return Ok(await _billerInvoiceServ.billerByAgentSumBiller(begenDate, endDate, idAgent, fkIdAgency));

        }
       

    }
}
