using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
using APIEndpoints;
using AutoMapper;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Orders
{
    public class List : BaseAsyncEndpoint.WithRequest<OrderListPageRequest>.WithResponse<List<OrderListResult>>
    {
        private readonly IAsyncRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public List(IAsyncRepository<Order> repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<List>();
        }

        [HttpGet("api/v1/orders", Name = "GetOrderList")]
        [SwaggerOperation(Summary = "Gets a list of orders", Description = "List of orders", OperationId = "Order.List", Tags = new[] {"Orders"})]
        [ProducesResponseType(typeof(List<OrderListResult>), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<List<OrderListResult>>> HandleAsync(
            [FromQuery] OrderListPageRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.OrderGetListEvent, "Getting list of orders");

            var orderList = await _repository.ListAllAsync(request.PerPage, request.Page, cancellationToken: cancellationToken);
            return Ok(orderList.Select(x => _mapper.Map<OrderListResult>(x)));
        }
    }
}