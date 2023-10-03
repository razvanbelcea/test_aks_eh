using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eathappy.order.business.External.DynamicPlatform.Interfaces
{
    public interface IAuthenticationApi
    {
        [Get]
        Task<AuthResponse> Authenticate([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }

    public class AuthResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
    }
}
