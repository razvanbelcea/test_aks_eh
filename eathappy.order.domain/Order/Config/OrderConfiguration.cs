using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eathappy.order.domain.Order.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", Order.DefaultSchema);
            builder.HasKey(c => c.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder
                .Property(x => x.State)
                .HasDefaultValue(OrderState.New)
                .HasConversion<string>();

            builder
                .HasMany(x => x.Articles)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder
                .Property(x => x.OrderDate)
                .HasColumnType("date");


            builder
                .Property(x => x.DeliveryDate)
                .HasColumnType("date");
        }
    }
}
