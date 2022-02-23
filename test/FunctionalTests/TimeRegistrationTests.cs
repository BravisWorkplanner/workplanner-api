using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using API.V1.Features.TimeRegistrations.Requests;
using Domain.Entities;
using FunctionalTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FunctionalTests
{
    [Collection(nameof(TestFixture))]
    public class TimeRegistrationTests
    {
        private readonly TestFixture _testFixture;

        public TimeRegistrationTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task CreateTimeRegistration_Should_Create_New_TimeRegistration()
        {
            var order = await _testFixture.ExecuteDbContextAsync(async db =>
            {
                var order = new Order();
                db.Entry(order).State = EntityState.Added;
                await db.SaveChangesAsync();
                return order;
            });

            var worker = await _testFixture.ExecuteDbContextAsync(async db =>
            {
                var worker = new Worker();
                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();
                return worker;
            });

            var timeRegistration = new TimeRegistrationCreateRequest
            {
                OrderId = order.Id,
                WorkerId = worker.Id,
                Day = DateTime.Now,
                Hours = 5,
                Week = ISOWeek.GetWeekOfYear(DateTime.Now).ToString(),
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(timeRegistration), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/v1/timeregistrations", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

            var getTimeRegistration = await _testFixture.ExecuteDbContextAsync(async db => await db.TimeRegistrations.FindAsync(id));
            
            Assert.NotNull(getTimeRegistration);
            Assert.Equal(getTimeRegistration.Id, id);
            Assert.Equal(timeRegistration.Day, getTimeRegistration.Day);
            Assert.Equal(timeRegistration.Hours, getTimeRegistration.Hours);
            Assert.Equal(timeRegistration.WorkerId, getTimeRegistration.WorkerId);
            Assert.Equal(timeRegistration.Week, getTimeRegistration.Week);
            Assert.Equal(timeRegistration.OrderId, getTimeRegistration.OrderId);
        }

        [Fact]
        public async Task UpdateTimeRegistration_Should_Update_TimeRegistration_For_Id()
        {
            var timeRegistration = await _testFixture.ExecuteDbContextAsync(async db =>
            {
                var order = new Order();
                db.Entry(order).State = EntityState.Added;
                await db.SaveChangesAsync();

                var worker = new Worker();
                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();

                var timeRegistration = new TimeRegistration
                {
                    OrderId = order.Id,
                    WorkerId = worker.Id,
                    Day = DateTime.Now,
                    Week = ISOWeek.GetWeekOfYear(DateTime.Now).ToString(),
                    Hours = 5,
                };

                db.Entry(timeRegistration).State = EntityState.Added;
                await db.SaveChangesAsync();

                return timeRegistration;
            });

            var timeRegistrationUpdateRequest = new TimeRegistrationUpdateRequest
            {
                TimeRegistrationId = timeRegistration.Id,
                Day = DateTime.Now,
                Week = ISOWeek.GetWeekOfYear(DateTime.Now).ToString(),
                Hours = 5,
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(timeRegistrationUpdateRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("api/v1/timeregistrations", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

            var getTimeRegistration = await _testFixture.ExecuteDbContextAsync(async db => await db.TimeRegistrations.FindAsync(id));

            Assert.NotNull(getTimeRegistration);
            Assert.Equal(getTimeRegistration.Id, id);
            Assert.Equal(timeRegistrationUpdateRequest.TimeRegistrationId, getTimeRegistration.Id);
            Assert.Equal(timeRegistrationUpdateRequest.Day, getTimeRegistration.Day);
            Assert.Equal(timeRegistrationUpdateRequest.Week, getTimeRegistration.Week);
            Assert.Equal(timeRegistrationUpdateRequest.Hours, getTimeRegistration.Hours);
        }

        [Fact]
        public async Task UpdateTimeRegistration_Should_Return_NotFound_For_Non_Existing_Id()
        {
            var timeRegistrationUpdateRequest = new TimeRegistrationUpdateRequest
            {
                TimeRegistrationId = int.MaxValue,
                Day = DateTime.Now,
                Week = ISOWeek.GetWeekOfYear(DateTime.Now).ToString(),
                Hours = 5,
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(timeRegistrationUpdateRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("api/v1/timeregistrations", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
