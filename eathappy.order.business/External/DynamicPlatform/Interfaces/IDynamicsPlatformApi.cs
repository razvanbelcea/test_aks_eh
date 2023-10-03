using eathappy.order.domain.Dtos.DynamicsPlatform;
using RestEase;
using System.Threading.Tasks;

namespace eathappy.order.business.External.DynamicPlatform.Interfaces
{
    public interface IDynamicsPlatformApi
    {
        [Get("data/v9.1/cr4b7_standortposes?$select=cr4b7_kostenstelle&$expand=cr4b7_flinkcode($select=cr4b7_code)&$filter=cr4b7_flinkcode ne null")]
        Task<StoreDetailsDto> GetStoreDetails([Header("Authorization")] string authorization);

        [Get("data/v9.2/cr4b7_mappingproduktionsshoptruhes?$select=cr4b7_Truhe&$expand=cr4b7_Produktionsshop($select=cr4b7_bezeichnung),cr4b7_Truhe($select=cr4b7_bezeichnung,cr4b7_kostenstelle)&$filter=cr4b7_gueltig eq true and contains(cr4b7_Produktionsshop/cr4b7_bezeichnung,'Shop') and contains(cr4b7_Truhe/cr4b7_bezeichnung,'Flink')")]
        Task<StoreServedHubDto> GetStoreServedHubs([Header("Authorization")] string authorization);
    }
}
