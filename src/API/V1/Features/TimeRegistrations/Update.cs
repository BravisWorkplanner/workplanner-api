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
    public class Update : BaseAsyncEndpoint.WithRequest<TimeRegistrationUpdateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;

        public Update(AppDbContext db)
        {
            _db = db;
        }

        [HttpPut("api/v1/timeregistrations", Name = "UpdateTimeRegistration")]
        [SwaggerOperation(
            Summary = "Update a Time registration",
            Description = "Update a Time registration",
            OperationId = "TimeRegistration.Update",
            Tags = new[] { "TimeRegistrations" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] TimeRegistrationUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var timeRegistration = await _db.FindAsync<TimeRegistration>(
                new object[] { request.TimeRegistrationId },
                cancellationToken: cancellationToken);
            if (timeRegistration == null)
            {
                return NotFound();
            }

            timeRegistration.Day = request.Day;
            timeRegistration.Hours = request.Hours;
            timeRegistration.Week = request.Week;

            _db.Entry(timeRegistration).State = EntityState.Modified;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(timeRegistration.Id);
        }
    }
}