using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using API.V1.Features.Expenses.Responses;
using API.V1.Features.Orders.Response;
using APIEndpoints;
using AutoMapper;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.EF;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.Expenses
{
    public class List : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<List<ExpenseListResult>>
    {
        private readonly ExpenseRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public List(ExpenseRepository repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<List>();
        }

        [HttpGet("api/v1/expenses", Name = "GetExpenseList")]
        [SwaggerOperation(Summary = "Gets a list of expenses", Description = "List of expenses", OperationId = "Expense.List", Tags = new[] {"Expenses"})]
        [ProducesResponseType(typeof(List<ExpenseListResult>), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<List<ExpenseListResult>>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.ExpenseGetListEvent, "Getting list of expenses");

            var expenseList = await _repository.GetAllExpensesWithOrderAndWorkerAsync(cancellationToken);
            return Ok(expenseList.Select(x => _mapper.Map<ExpenseListResult>(x)));
        }
    }
}
