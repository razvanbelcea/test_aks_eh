using eathappy.order.data.Context;
using eathappy.order.data.Interfaces;
using eathappy.order.data.Interfaces.Specific;
using eathappy.order.data.Repositories;
using eathappy.order.data.Repositories.Specific;
using eathappy.order.domain.Article;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace eathappy.order.data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        #region Repositories
        private readonly IOrderRepository orderRepository;
        public IOrderRepository OrderRepository => orderRepository ?? new OrderRepository(_context);

        private readonly IRepository<Article> articleRepository;
        public IRepository<Article> ArticleRepository => articleRepository ?? new Repository<Article>(_context);
        #endregion

        public DatabaseFacade Database => _context.Database;

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context.Dispose();
    }
}
