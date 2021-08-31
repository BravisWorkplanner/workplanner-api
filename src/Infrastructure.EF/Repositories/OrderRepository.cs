using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories
{
    public sealed class OrderRepository : EfRepository<Order>
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> GetOrderByIdWithExpensesAndTimeRegistrationsAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                                   .Include(x => x.Expenses)
                                   .ThenInclude(x => x.Worker)
                                   .Include(x => x.TimeRegistrations)
                                   .ThenInclude(x => x.Worker)
                                   .AsSplitQuery()
                                   .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
