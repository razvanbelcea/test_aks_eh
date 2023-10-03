using eathappy.order.domain.Dtos.Local.Result;

namespace eathappy.order.domain.Models
{
    public class AuthenticationResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
