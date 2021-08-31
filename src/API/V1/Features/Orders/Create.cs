using API.V1.Features.Orders.Request;
using APIEndpoints;
using AutoMapper;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Orders
{
    public class Create : BaseAsyncEndpoint.WithRequest<OrderCreateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Create(IAsyncRepository<Order> repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<Create>();
        }

        [HttpPost("api/v1/orders", Name = "CreateOrder")]
        [SwaggerOperation(Summary = "Create a new Order", Description = "Create a new Order", OperationId = "Order.Create", Tags = new[] {"Orders"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] OrderCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.OrderCreateEvent, "Creating order with object number {ObjNbr}", request.ObjectNumber);

            var order = _mapper.Map<Order>(request);

            var result = await _repository.AddAsync(order, cancellationToken);

            return CreatedAtRoute("GetOrder", new {id = result.Id}, result);
        }
    }
}