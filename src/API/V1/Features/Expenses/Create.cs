using System;
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
    public class Create : BaseAsyncEndpoint.WithRequest<OrderExpenseCreateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;

        public Create(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("api/v1/expense", Name = "CreateExpense")]
        [SwaggerOperation(
            Summary = "Create a new Expense",
            Description = "Create a new Expense",
            OperationId = "Expense.Create",
            Tags = new[] { "Expenses" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] OrderExpenseCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var expense = new Expense
            {
                OrderId = request.OrderId,
                WorkerId = request.WorkerId,
                Price = request.Price,
                Description = request.Description,
                ProductId = request.ProductId,
                InvoiceId = request.InvoiceId,
                CreatedAt = DateTime.UtcNow,
            };

            _db.Entry(expense).State = EntityState.Added;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(expense.Id);
        }
    }
}