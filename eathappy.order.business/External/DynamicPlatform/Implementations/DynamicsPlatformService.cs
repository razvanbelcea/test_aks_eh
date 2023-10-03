using eathappy.order.business.External.DynamicPlatform.Interfaces;
using eathappy.order.domain.Dtos.Configurations;
using eathappy.order.domain.Dtos.DynamicsPlatform;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RestEase;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace eathappy.order.business.External.DynamicPlatform.Implementations
{
    public class DynamicsPlatformService : IDynamicsPlatformService
    {
        private readonly IDynamicsPlatformApi _dynamicsPlatformApi;
        private readonly IAuthenticationApi _authenticationApi;
        private readonly DynamicsPlatformConfig _dynamicsConfig;
        private AsyncRetryPolicy _retryPolicy;
        private string _token = "Bearer ";

        public DynamicsPlatformService(IOptions<DynamicsPlatformConfig> dynamicsConfig)
        {
            SetupRetryPolicy();
            _dynamicsConfig = dynamicsConfig.Value;
            _authenticationApi = RestClient.For<IAuthenticationApi>(_dynamicsConfig.AuthApi);
            _dynamicsPlatformApi = RestClient.For<IDynamicsPlatformApi>(_dynamicsConfig.DynamicsPlatformApi);

        }
        public async Task<StoreDetailsDto> GetStoreDetails()
        {
            var response = await _retryPolicy
                .ExecuteAsync(async () => await _dynamicsPlatformApi.GetStoreDetails(_token));

            return response;
        }

        public async Task<StoreServedHubDto> GetStoreServedHubs()
        {
            var response = await _retryPolicy
                .ExecuteAsync(async () => await _dynamicsPlatformApi.GetStoreServedHubs(_token));

            return response;
        }

        private async Task Authenticate()
        {
            var data = new Dictionary<string, object> {
                    {nameof(_dynamicsConfig.resource), _dynamicsConfig.resource},
                    {nameof(_dynamicsConfig.grant_type), _dynamicsConfig.grant_type },
                    {nameof(_dynamicsConfig.client_id), _dynamicsConfig.client_id},
                    {nameof(_dynamicsConfig.client_secret), _dynamicsConfig.client_secret}
            };

            var response = await _authenticationApi.Authenticate(data);

            if (response != null)
                _token += response.access_token;
        }

        private void SetupRetryPolicy()
        {
            _retryPolicy = Policy
                 .Handle<ApiException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
                 .RetryAsync(async (exception, retryCount) =>
                 {
                     await Authenticate();
                 });
        }
    }
}
