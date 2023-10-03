using eathappy.order.domain.Dtos.DynamicsPlatform;
using System.Threading.Tasks;

namespace eathappy.order.business.External.DynamicPlatform.Interfaces
{
    public interface IDynamicsPlatformService
    {
        Task<StoreDetailsDto> GetStoreDetails();
        Task<StoreServedHubDto> GetStoreServedHubs();
    }
}
