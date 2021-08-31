using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using APIEndpoints;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.Expenses
{
    public class Update : BaseAsyncEndpoint.WithRequest<OrderExpenseUpdateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Expense> _repository;

        public Update(IAsyncRepository<Expense> repository)
        {
            _repository = repository;
        }

        [HttpPut("api/v1/expense", Name = "UpdateExpense")]
        [SwaggerOperation(Summary = "Update a Expense", Description = "Update a Expense", OperationId = "Expense.Update", Tags = new[] {"Expenses"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(OrderExpenseUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var expense = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = request.Description;
            expense.Price = request.Price;

            await _repository.UpdateAsync(expense, cancellationToken);

            return Ok(expense.Id);
        }
    }
}