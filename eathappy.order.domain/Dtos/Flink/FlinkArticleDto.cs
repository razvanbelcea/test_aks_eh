using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;

namespace eathappy.order.domain.Dtos.Flink
{
    public class FlinkArticleDto
    {
        [Name("hub")]
        public string Hub { get; set; }
        [Name("customer_id")]
        public string Customer { get; set; }
        [Name("order_date")]
        [Format("yyyy-MM-dd")]
        public DateTime OrderDate { get; set; }
        [Name("delivery_date")]
        [Format("yyyy-MM-dd")]
        public DateTime DeliveryDate { get; set; }
        [Name("eathappy_sku"), Optional]
        [Default(0)]
        public int EatHappySku { get; set; }
        [Name("flink_sku"), Optional]
        [Default(0)]
        public int FlinkSku { get; set; }
        [Name("sku"), Optional]
        [Default(0)]
        public int Sku { get; set; }
        [Name("desc")]
        public string Description { get; set; }
        [Name("order_hu_qty", "hu_quantity"), Optional]
        public decimal Quantity { get; set; }
        [Name("edi"), Optional]
        [Default(0)]
        public long EdiNumber { get; set; }
    }
    //public sealed class FlinkArticleDtoMap : ClassMap<FlinkArticleDto>
    //{
    //    public FlinkArticleDtoMap()
    //    {
    //        Map(m => m.Hub).Name("hub");
    //        Map(m => m.Customer).Name("customer_id");
    //        Map(m => m.OrderDate).Name("order_date");
    //        Map(m => m.DeliveryDate).Name("delivery_date");
    //        Map(m => m.EatHappySku).Name("eathappy_sku");
    //        Map(m => m.FlinkSku).Name("flink_sku");
    //        Map(m => m.Sku).Name("sku");
    //        Map(m => m.Description).Name("desc");
    //        Map(m => m.Quantity).Name("order_hu_qty", "hu_quantity");
    //        Map(m => m.EdiNumber).Name("edi").Optional();
    //    }
    //}
}
