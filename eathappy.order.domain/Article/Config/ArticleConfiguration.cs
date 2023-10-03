using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static eathappy.order.domain.Article.ArticleEnums;

namespace eathappy.order.domain.Article.Config
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("articles", Article.DefaultSchema);
            builder.HasKey(c => c.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder
                .Property(x => x.Status)
                .HasDefaultValue(ArticleStatus.Valid)
                .HasConversion<string>();

            builder
                .Property(x => x.ReasonCode)
                .HasDefaultValue(ReasonCode.None)
                .HasConversion<string>();

            builder
                .Property(x => x.Quantity)
                .HasColumnType("decimal(18,2)");
        }
    }
}
