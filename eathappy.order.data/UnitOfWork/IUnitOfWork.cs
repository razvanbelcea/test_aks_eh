using eathappy.order.data.Interfaces;
using eathappy.order.data.Interfaces.Specific;
using eathappy.order.domain.Article;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace eathappy.order.data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        IRepository<Article> ArticleRepository { get; }
        DatabaseFacade Database { get; }
        Task<bool> SaveChangesAsync();
    } 
}
