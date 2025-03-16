using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface CustomerWalletRep
    {
        Task<ResponseBody<CustomerWallet>> addCustomer(CustomerWallet cus);
        Task<ResponseBody<CustomerWallet>> CustomerToAgencyDto(CustomerToAgencyDto dto);
        Task<ResponseBody<CustomerWallet>> addCustomerWithAccounting(BodyCustomerWalletDto cus);
        Task<ResponseBody<CustomerWallet>> definePercentagePurchase(DefinePercentagePurchaseMasterDto dto);
        Task<ResponseBody<CustomerWallet>> updateCustomerInformations(CustomerUpdateInfosDto cus);
        Task<ResponseBody<CustomerWallet>> updateCustomerInformationsWithoutPinNumber(CustomerUpdateDto cus);
        Task<ResponseBody<CustomerWallet>> updateCustomerPin(CustomerUpadatePinDto cus);
        Task<ResponseBody<String>> updateCustomerPinNumberByStaff(UpdateCustomerByStaffDto cus);
        Task<ResponseBody<CustomerWallet>> addCustomerWithAccountingPhoneOnly(BodyCustomerPhoneDto cus);
        Task<ResponseBody<List<CustomerWallet>>> findAllCustomerByprofile(string profile, Ulid? idAgency, String? phone, int page = 1, int limit = 10);
        Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(int page = 1, int limit = 10);
        Task<ResponseBody<List<CustomerWallet>>> findAllAgentsByAgency(Ulid? idAgency, int page = 1, int limit = 10);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletContryidentityAndPhone(BodyPhoneShDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletPinContryidentityPhone(BodyLoginDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletPintempContryidentityPhone(BodyConfPinTempDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletCode(string code);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletById(Ulid id);
        Task<ResponseBody<CustomerWallet>> activateWallet(ActiveBlockDto dto);
        Task<ResponseBody<CustomerWallet>> blokeOrDeblokeWallet(ActiveBlockDto id);
        Task<ResponseBody<List<CustomerWallet>>> getCustomerWalletDateBetweenAndAgent(DateTime begenDate, DateTime endDate, Ulid? idAgent, int page = 1, int limit = 10);


    }
}
