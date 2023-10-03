using Microsoft.AspNetCore.Identity;

namespace eathappy.order.domain.User
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
