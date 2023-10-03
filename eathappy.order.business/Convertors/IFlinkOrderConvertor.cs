using eathappy.order.domain.Article;
using eathappy.order.domain.Order;
using System.Collections.Generic;

namespace eathappy.order.business.Convertors
{
    public interface IFlinkOrderConvertor
    {
        (Order, IEnumerable<Article>) ConvertFlinkOrder(byte[] flinkOrder);
    }
}
