using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eathappy.order.domain.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Name = RoleConstants.Viewer,
                NormalizedName = RoleConstants.Viewer.ToUpper()
            },
            new IdentityRole
            {
                Name = RoleConstants.StoreApprover,
                NormalizedName = RoleConstants.StoreApprover.ToUpper()
            },
            new IdentityRole
            {
                Name = RoleConstants.HubApprover,
                NormalizedName = RoleConstants.HubApprover.ToUpper()
            },
            new IdentityRole
            {
                Name = RoleConstants.Administrator,
                NormalizedName = RoleConstants.Administrator.ToUpper()
            }
        );
        }
    }
}
