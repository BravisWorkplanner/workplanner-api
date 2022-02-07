using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using APIEndpoints;
using Domain.Entities;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.Expenses
{
    public class Update : BaseAsyncEndpoint.WithRequest<ExpenseUpdateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;

        public Update(AppDbContext db)
        {
            _db = db;
        }

        [HttpPut("api/v1/expense", Name = "UpdateExpense")]
        [SwaggerOperation(
            Summary = "Update a Expense",
            Description = "Update a Expense",
            OperationId = "Expense.Update",
            Tags = new[] { "Expenses" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] ExpenseUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var expense = await _db.FindAsync<Expense>(
                new object[] { request.ExpenseId },
                cancellationToken: cancellationToken);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = request.Description;
            expense.Price = request.Price;
            expense.InvoiceId = request.InvoiceId;

            _db.Entry(expense).State = EntityState.Modified;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(expense.Id);
        }
    }
}