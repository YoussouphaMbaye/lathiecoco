﻿using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface InvoiceStartupMasterRep
    {
        Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMaster(BodyInvoiceStartupMaster ism);
        Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMasterByAgency(BodyInvoiceStartupMasterByAgent ism);
        Task<ResponseBody<InvoiceStartupMaster>> ValidateInvoiceStartupMaster(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster);
        Task<ResponseBody<InvoiceStartupMaster>> validateInvoiceStartupMasterOfAgency(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster);

        Task<ResponseBody<List<InvoiceStartupMaster>>> findAllInvoiceStartupMasterAgency(int page = 1, int limit = 10);
        Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByMaster(Ulid fkIdMaster, int page = 1, int limit = 10);
        Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByOwnerAgent(Ulid fkIdOwnerAgent,int page = 1, int limit = 10);
        Task<ResponseBody<InvoiceStartupMaster>> uploadProof(IFormFile formFile, Ulid idInvoiceStartupMaster);
        Task<ResponseBody<List<InvoiceStartupMaster>>> searcheInvoiceStartupMaster(string? status, string? code, DateTime? beginDate, DateTime? endDate, int page, int limit);
        Task<ResponseBody<InvoiceStartupMaster>> findInvoiceStarupMasterById(Ulid id);
        Task<ResponseBody<InvoiceStartupMaster>> ValidateInvoiceStartupMasterByAgencyUser(ValidateInvoiceStartupMasterDto dto);
        Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByAgency(Ulid idAgency, int page = 1, int limit = 10);
        Task<ResponseBody<string>> getFileUrl(string key);

        //Task<ResponseBody<List<InvoiceMasterAgency>>> findInvoiceMasterAgencyByMasterAgency(Ulid IdMasterAgency, int page = 1, int limit = 10);
    }
}
