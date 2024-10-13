using Lathiecoco.dto;
using Lathiecoco.models;

namespace Lathiecoco.repository
{
    public interface InvoiceStartupMasterRep
    {
        Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMaster(BodyInvoiceStartupMaster ism);
        Task<ResponseBody<InvoiceStartupMaster>> ValidateInvoiceStartupMaster(BodyValidInvoiceStartupMaster bodyValidInvoiceStartupMaster);
        Task<ResponseBody<List<InvoiceStartupMaster>>> findAllInvoiceStartupMasterAgency(int page = 1, int limit = 10);
        Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByMaster(Ulid fkIdMaster, int page = 1, int limit = 10);

        Task<ResponseBody<List<InvoiceStartupMaster>>> findInvoiceStartupByOwnerAgent(Ulid fkIdOwnerAgent,int page = 1, int limit = 10);
        Task<ResponseBody<InvoiceStartupMaster>> initiateInvoiceStarupMasterById(Ulid id);

        //Task<ResponseBody<List<InvoiceMasterAgency>>> findInvoiceMasterAgencyByMasterAgency(Ulid IdMasterAgency, int page = 1, int limit = 10);
    }
}
