using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories
{
    public class ExpenseRepository : EfRepository<Expense>
    {
        private readonly AppDbContext _dbContext;

        public ExpenseRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Expense>> GetAllExpensesWithOrderAndWorkerAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Expenses
                                   .Include(x => x.Worker)
                                   .Include(x => x.Order)
                                   .ToListAsync(cancellationToken);
        }
    }
}
