using Lathiecoco.dto;
using Lathiecoco.models;

namespace  Lathiecoco.repository
{
    public interface CustomerWalletRep
    {
        Task<ResponseBody<CustomerWallet>> addCustomer(CustomerWallet cus);
        Task<ResponseBody<CustomerWallet>> addCustomerWithAccounting(BodyCustomerWalletDto cus);
        Task<ResponseBody<CustomerWallet>> updateCustomerInformations(CustomerUpdateInfosDto cus);
        Task<ResponseBody<CustomerWallet>> updateCustomerInformationsWithoutPinNumber(CustomerUpdateDto cus);
        Task<ResponseBody<CustomerWallet>> updateCustomerPin(CustomerUpadatePinDto cus);
        Task<ResponseBody<CustomerWallet>> addCustomerWithAccountingPhoneOnly(BodyCustomerPhoneDto cus);
        Task<ResponseBody<List<CustomerWallet>>> findAllCustomerByprofile(string profile,int page = 1, int limit = 10);
        Task<ResponseBody<List<CustomerWallet>>> findAllCustomer(int page = 1, int limit = 10);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletContryidentityAndPhone(BodyPhoneShDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletPinContryidentityPhone(BodyLoginDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletPintempContryidentityPhone(BodyConfPinTempDto bd);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletCode(string code);
        Task<ResponseBody<CustomerWallet>> findCustomerWalletById(Ulid id);
        Task<ResponseBody<CustomerWallet>> activateWallet(Ulid id);
        Task<ResponseBody<CustomerWallet>> blokeOrDeblokeWallet(Ulid id);

    }
}
