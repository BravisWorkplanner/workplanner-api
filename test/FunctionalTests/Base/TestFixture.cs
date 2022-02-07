using API;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Xunit;

namespace FunctionalTests.Base
{
    [CollectionDefinition(nameof(TestFixture))]
    public class SliceFixtureCollection : ICollectionFixture<TestFixture>
    {
    }

    public class TestFixture
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TestFixture()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
            _scopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public CustomWebApplicationFactory<Startup> Factory { get; }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                await dbContext.BeginTransactionAsync();

                await action(scope.ServiceProvider);

                await dbContext.CommitTransactionAsync();
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();

                throw;
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                await dbContext.BeginTransactionAsync();

                var result = await action(scope.ServiceProvider);

                await dbContext.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();

                throw;
            }
        }

        public Task ExecuteDbContextAsync(Func<AppDbContext, Task> action) =>
            ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));

        public Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, Task<T>> action) =>
            ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));

        private int _nextObjectNumber = 1;

        public int NextCourseNumber() => Interlocked.Increment(ref _nextObjectNumber);

        public Order CreateTestOrder()
        {
            var orderFaker = new Faker<Order>().RuleFor(x => x.ObjectNumber, _ => $"B-00{NextCourseNumber()}").
                                                RuleFor(x => x.Address, f => f.Person.Address.Street).
                                                RuleFor(x => x.Description, f => f.Lorem.Sentence(7)).
                                                RuleFor(x => x.StartDate, f => f.Date.Soon(7)).
                                                RuleFor(x => x.EndDate, f => f.Date.Soon(31)).
                                                RuleFor(x => x.InvoiceDate, f => f.Date.Future(45)).
                                                RuleFor(x => x.OrderStatus, f => (OrderStatus)f.Random.Int(0, 3));

            var workerFaker = new Faker<Worker>().RuleFor(x => x.Name, f => f.Person.FullName).
                                                  RuleFor(x => x.Company, f => f.Company.CompanyName()).
                                                  RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("####-######"));

            var productFaker = new Faker<Product>().RuleFor(x => x.Type, f => f.Name.JobType()).
                                                    RuleFor(x => x.Description, f => f.Name.JobType());

            var expenseFaker = new Faker<Expense>().RuleFor(x => x.Description, f => f.Lorem.Sentence()).
                                                    RuleFor(x => x.Price, f => Convert.ToDouble(f.Commerce.Price()));

            var timeRegistrationFaker = new Faker<TimeRegistration>().RuleFor(
                                                                          x => x.Week,
                                                                          f =>
                                                                          {
                                                                              var date = f.Date.Soon(1);

                                                                              return
                                                                                  $"{ISOWeek.GetYear(date)}{ISOWeek.GetWeekOfYear(date)}";
                                                                          }).
                                                                      RuleFor(x => x.Day, _ => DateTime.UtcNow);

            var order = orderFaker.Generate();
            var worker = workerFaker.Generate();
            var expenses = Enumerable.Range(0, 10).Select(_ => expenseFaker.Generate()).ToList();
            var timeRegistrations = Enumerable.Range(0, 10).Select(_ => timeRegistrationFaker.Generate()).ToList();
            var products = Enumerable.Range(0, 10).Select(_ => productFaker.Generate()).ToList();

            var rand = new Random();
            timeRegistrations.ForEach(
                x =>
                {
                    x.Worker = worker;
                    x.Order = order;
                });
            expenses.ForEach(
                x =>
                {
                    x.Worker = worker;
                    x.Order = order;
                    x.Product = products.ElementAt(rand.Next(0, products.Count - 1));
                });

            order.Expenses = expenses;
            order.TimeRegistrations = timeRegistrations;

            return order;
        }
    }
}