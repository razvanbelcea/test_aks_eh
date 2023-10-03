using eathappy.order.domain.Article;
using eathappy.order.domain.Article.Config;
using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Config;
using eathappy.order.domain.Roles;
using eathappy.order.domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eathappy.order.data.Context
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .ApplyConfiguration(new OrderConfiguration())
                .ApplyConfiguration(new ArticleConfiguration())
                .ApplyConfiguration(new RoleConfiguration());
        }
    }
}
