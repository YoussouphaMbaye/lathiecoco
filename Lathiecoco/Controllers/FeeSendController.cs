﻿using Lathiecoco.models;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace  Lathiecoco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeSendController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;

        private readonly IConfiguration _configuration;

        private FeesSendRep _feeSendRepService;
        public FeeSendController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement,

            IConfiguration configuration,
            FeesSendRep feeSendRepService)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;

            _configuration = configuration;

            _feeSendRepService = feeSendRepService;
        }
        [HttpPost("/fee-send")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<FeeSend>> PostFeeSend([FromBody] FeeSend ac)
        {

            return await _feeSendRepService.addFeesSend(ac);

        }
        [HttpPost("/fee-send/limit-update")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<FeeSend>> limitUpdate([FromBody] FeeLimitUpdateDto feeLimitUpdateDto)
        {

            return await _feeSendRepService.updateLimitFeesSend(feeLimitUpdateDto);

        }
        [HttpGet("/fee-send/find-with-payment-mode")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<FeeSend>> findWithPaymentMode(Ulid id)
        {

            return await _feeSendRepService.findWithPaymentMode(id);

        }
       
        [HttpGet("/fee-send/find-all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ResponseBody<List<FeeSend>>> findAllFeesSend(int page = 1, int limit = 10)
        {

            return await _feeSendRepService.findAllFeeSend(page, limit);

        }

    }
}
