using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using APIEndpoints;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.Expenses
{
    public class Create : BaseAsyncEndpoint.WithRequest<OrderExpenseCreateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Expense> _repository;
        private readonly IMapper _mapper;

        public Create(IAsyncRepository<Expense> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("api/v1/expense", Name = "CreateExpense")]
        [SwaggerOperation(
            Summary = "Create a new Expense",
            Description = "Create a new Expense",
            OperationId = "Expense.Create",
            Tags = new[] {"Expenses"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(OrderExpenseCreateRequest request, CancellationToken cancellationToken = default)
        {
            var expense = _mapper.Map<Expense>(request);

            var result = await _repository.AddAsync(expense, cancellationToken);

            return result.Id;
        }
    }
}