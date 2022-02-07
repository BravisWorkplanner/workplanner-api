using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.TimeRegistrations.Requests;
using APIEndpoints;
using Domain.Entities;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.TimeRegistrations
{
    public class Create : BaseAsyncEndpoint.WithRequest<TimeRegistrationCreateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;

        public Create(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("api/v1/timeregistrations", Name = "CreateTimeRegistration")]
        [SwaggerOperation(
            Summary = "Create a new TimeRegistration",
            Description = "Create a new TimeRegistration",
            OperationId = "TimeRegistration.Create",
            Tags = new[] { "TimeRegistrations" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] TimeRegistrationCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var timeRegistration = new TimeRegistration
            {
                OrderId = request.OrderId,
                WorkerId = request.WorkerId,
                Day = request.Day,
                CreatedAt = DateTime.UtcNow,
                Hours = request.Hours,
                Week = request.Week,
            };

            _db.Entry(timeRegistration).State = EntityState.Added;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(timeRegistration.Id);
        }
    }
}