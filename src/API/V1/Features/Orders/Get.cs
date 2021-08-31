using API.V1.Features.Orders.Response;
using APIEndpoints;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.EF.Repositories;

namespace API.V1.Features.Orders
{
    public class Get : BaseAsyncEndpoint.WithRequest<int>.WithResponse<OrderGetResult>
    {
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Get(OrderRepository orderRepository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<Get>();
        }

        [HttpGet("api/v1/orders/{id}", Name = "GetOrder")]
        [SwaggerOperation(Summary = "Get a specific Order", Description = "Get a specific Order", OperationId = "Order.Get", Tags = new[] {"Orders"})]
        [ProducesResponseType(typeof(OrderGetResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.NotFound)]
        public override async Task<ActionResult<OrderGetResult>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.OrderGetEvent, "Getting order with id {OrderId}", id);

            var order = await _orderRepository.GetOrderByIdWithExpensesAndTimeRegistrationsAsync(id, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            return Ok(_mapper.Map<OrderGetResult>(order));
        }
    }
}