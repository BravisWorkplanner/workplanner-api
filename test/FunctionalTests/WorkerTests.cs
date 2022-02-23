using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.V1.Features.Workers.Request;
using API.V1.Features.Workers.Response;

using Domain.Entities;

using FunctionalTests.Base;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace FunctionalTests;

[Collection(nameof(TestFixture))]
public class WorkerTests
{
    private readonly TestFixture _testFixture;

    public WorkerTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task CreateWorker_Should_Create_New_Worker()
    {
        // arrange
        var createdWorker = new WorkerCreateRequest()
        {
            Name = Faker.Name.FullName(),
            Company = Faker.Company.Name(),
            PhoneNumber = "0700123123",
        };

        var httpClient = _testFixture.Factory.CreateClient();
        var httpContent = new StringContent(JsonSerializer.Serialize(createdWorker), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PostAsync("api/v1/workers", httpContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

        Assert.NotEqual(default(int), id);

        var worker = await _testFixture.ExecuteDbContextAsync(async db => await db.Workers.FindAsync(id));

        Assert.NotNull(worker);
        Assert.Equal(id, worker.Id);
    }

    [Fact]
    public async Task CreateWorker_Should_Return_BadRequest_For_Incorrect_RequestBody()
    {
        // arrange
        var createdWorker = new WorkerCreateRequest
        {
            Name = Faker.Name.FullName(),
            Company = Faker.Company.Name(),
            PhoneNumber = "0700-123123",
        };

        var httpClient = _testFixture.Factory.CreateClient();
        var httpContent = new StringContent(JsonSerializer.Serialize(createdWorker), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PostAsync("api/v1/workers", httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorker_Should_Delete_Worker()
    {
        // arrange
        var worker = await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var worker = new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                };

                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();

                return worker;
            });

        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.DeleteAsync($"api/v1/workers/{worker.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var deletedWorker = await _testFixture.ExecuteDbContextAsync(async db => await db.Workers.FindAsync(worker.Id));

        Assert.Null(deletedWorker);
    }

    [Fact]
    public async Task DeleteWorker_Should_Return_NotFound_For_Non_Existing_Worker()
    {
        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.DeleteAsync($"api/v1/workers/{int.MaxValue}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWorker_Should_Update_Worker()
    {
        var worker = await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var worker = new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                };

                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();

                return worker;
            });

        var updateWorkerRequest = new WorkerUpdateRequest
        {
            WorkerId = worker.Id,
            Name = Faker.Name.FullName(),
            Company = Faker.Company.Name(),
            PhoneNumber = "0700123123",
        };
        var httpClient = _testFixture.Factory.CreateClient();

        var data = new StringContent(JsonSerializer.Serialize(updateWorkerRequest), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PutAsync("api/v1/workers", data);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedWorker = await _testFixture.ExecuteDbContextAsync(async db => await db.Workers.FindAsync(worker.Id));

        Assert.NotNull(updatedWorker);
        Assert.Equal(updateWorkerRequest.WorkerId, updatedWorker.Id);
        Assert.Equal(updateWorkerRequest.Company, updatedWorker.Company);
        Assert.Equal(updateWorkerRequest.Name, updatedWorker.Name);
        Assert.Equal(updateWorkerRequest.PhoneNumber, updatedWorker.PhoneNumber);
    }

    [Fact]
    public async Task UpdateWorker_Should_Return_NotFound_For_Non_Existing_Worker()
    {
        var httpClient = _testFixture.Factory.CreateClient();

        var updateWorkerRequest = new WorkerUpdateRequest
        {
            WorkerId = int.MaxValue,
            Name = Faker.Name.FullName(),
            Company = Faker.Company.Name(),
            PhoneNumber = "0700123123",
        };

        var data = new StringContent(JsonSerializer.Serialize(updateWorkerRequest), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PutAsync($"api/v1/workers", data);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetWorker_Should_Return_Worker_For_Given_Id()
    {
        var worker = await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var worker = new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                };

                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();

                return worker;
            });

        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.GetAsync($"api/v1/workers/{worker.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getWorker = JsonSerializer.Deserialize<WorkerGetResult>(await response.Content.ReadAsStreamAsync());

        Assert.NotNull(getWorker);
        Assert.Equal(worker.Id, getWorker.WorkerId);
        Assert.Equal(worker.Company, getWorker.Company);
        Assert.Equal(worker.Name, getWorker.Name);
        Assert.Equal(worker.PhoneNumber, getWorker.PhoneNumber);
    }

    [Fact]
    public async Task GetWorker_Should_Return_NotFound_For_Non_Existing_Worker()
    {
        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.GetAsync($"api/v1/workers/{int.MaxValue}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ListAll_Should_Return_List_Of_All_Workers()
    {
        // arrange
        await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var worker1 = new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                };

                var worker2 = new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                };

                db.Workers.AddRange(worker1, worker2);
                await db.SaveChangesAsync();
            });

        // act
        var httpClient = _testFixture.Factory.CreateClient();
        var response = await httpClient.GetAsync("api/v1/workers");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var workerList = JsonSerializer.Deserialize<List<WorkerListResult>>(content);

        Assert.NotNull(workerList);
        Assert.NotEmpty(workerList);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public async Task ListAll_Should_Return_List_Of_Specified_PageSize(int pageSize)
    {
        // arrange
        await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var workers = Enumerable.Range(0, 5).Select(_ => new Worker
                {
                    Name = Faker.Name.FullName(),
                    Company = Faker.Company.Name(),
                    PhoneNumber = "0700123123",
                });

                db.Workers.AddRange(workers);
                await db.SaveChangesAsync();
            });

        // act
        var httpClient = _testFixture.Factory.CreateClient();
        var response = await httpClient.GetAsync($"api/v1/orders?perPage={pageSize}&page=1");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var workerList = JsonSerializer.Deserialize<List<WorkerGetResult>>(content);

        Assert.NotNull(workerList);
        Assert.Equal(workerList.Count, pageSize);
    }
}