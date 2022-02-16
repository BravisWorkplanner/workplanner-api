using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using API.V1.Features.Expenses.Requests;

using Domain.Entities;

using FunctionalTests.Base;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Xunit;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FunctionalTests
{
    [Collection(nameof(TestFixture))]
    public class ExpenseTests
    {
        private readonly TestFixture _testFixture;

        public ExpenseTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task CreateExpense_Should_Create_New_Expense()
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

            var product = await _testFixture.ExecuteDbContextAsync(async db =>
            {
                var product = new Product();
                db.Entry(product).State = EntityState.Added;
                await db.SaveChangesAsync();
                return product;
            });

            var expense = new OrderExpenseCreateRequest
            {
                OrderId = order.Id,
                ProductId = product.Id,
                WorkerId = worker.Id,
                Description = Faker.Lorem.GetFirstWord(),
                Price = 45.50,
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(expense), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/v1/expense", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

            var getExpense = await _testFixture.ExecuteDbContextAsync(async db => await db.Expenses.FindAsync(id));
            
            Assert.NotNull(getExpense);
            Assert.Equal(getExpense.Id, id);
            Assert.Equal(expense.Description, getExpense.Description);
            Assert.Equal(expense.InvoiceId, getExpense.InvoiceId);
            Assert.Equal(expense.WorkerId, getExpense.WorkerId);
            Assert.Equal(expense.ProductId, getExpense.ProductId);
            Assert.Equal(expense.OrderId, getExpense.OrderId);
            Assert.Equal(expense.Price, getExpense.Price);
        }

        [Fact]
        public async Task UpdateExpense_Should_Update_Expense_For_Id()
        {
            var expense = await _testFixture.ExecuteDbContextAsync(async db =>
            {
                var order = new Order();
                db.Entry(order).State = EntityState.Added;
                await db.SaveChangesAsync();

                var worker = new Worker();
                db.Entry(worker).State = EntityState.Added;
                await db.SaveChangesAsync();

                var product = new Product();
                db.Entry(product).State = EntityState.Added;
                await db.SaveChangesAsync();

                var expense = new Expense
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    WorkerId = worker.Id,
                    Description = Faker.Lorem.GetFirstWord(),
                    Price = 45.50,
                };

                db.Entry(expense).State = EntityState.Added;
                await db.SaveChangesAsync();

                return expense;
            });

            var expenseUpdateRequest = new ExpenseUpdateRequest
            {
                ExpenseId = expense.Id,
                InvoiceId = "123123",
                ProductId = expense.ProductId,
                Description = Faker.Lorem.GetFirstWord(),
                Price = 43.40,
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(expenseUpdateRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("api/v1/expense", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

            var getExpense = await _testFixture.ExecuteDbContextAsync(async db => await db.Expenses.FindAsync(id));

            Assert.NotNull(getExpense);
            Assert.Equal(getExpense.Id, id);
            Assert.Equal(expenseUpdateRequest.Description, getExpense.Description);
            Assert.Equal(expenseUpdateRequest.InvoiceId, getExpense.InvoiceId);
            Assert.Equal(expenseUpdateRequest.ProductId, getExpense.ProductId);
            Assert.Equal(expenseUpdateRequest.Price, getExpense.Price);
        }

        [Fact]
        public async Task UpdateExpense_Should_Return_NotFound_For_Non_Existing_Id()
        {
            var expenseUpdateRequest = new ExpenseUpdateRequest
            {
                ExpenseId = int.MaxValue,
                InvoiceId = "123123",
                ProductId = 1,
                Description = Faker.Lorem.GetFirstWord(),
                Price = 43.40,
            };

            var httpClient = _testFixture.Factory.CreateClient();

            var content = new StringContent(JsonSerializer.Serialize(expenseUpdateRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("api/v1/expense", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
