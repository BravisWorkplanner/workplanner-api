using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper.Internal;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
            SeedDatabase(dbContext, true);
        }

        public static void SeedDatabase(AppDbContext dbContext, bool forceSeed = false)
        {
            var alreadySeeded = CheckIfDatabaseIsAlreadySeeded(dbContext);
            if (alreadySeeded && forceSeed == false)
            {
                return;
            }

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            var orders = SeedOrderTable(dbContext);
            var workers =  SeedWorkerTable(dbContext);
            SeedExpenseTable(dbContext, orders, workers);
            SeedTimeRegistrationTable(dbContext, orders, workers);

            dbContext.SaveChanges();
        }

        private static bool CheckIfDatabaseIsAlreadySeeded(AppDbContext dbContext)
        {
            return dbContext.Orders.Any() && dbContext.TimeRegistrations.Any() && dbContext.Expenses.Any() && dbContext.Workers.Any();
        }

        private static IReadOnlyCollection<Order> SeedOrderTable(AppDbContext dbContext)
        {
            var orderIds = 900;
            List<Order> orderList = new();
            var order = new Faker<Order>().RuleFor(x => x.ObjectNumber, _ => $"B-{orderIds++}").
                                           RuleFor(x => x.Address, f => f.Person.Address.Street).
                                           RuleFor(x => x.Description, f => f.Lorem.Sentence(7)).
                                           RuleFor(x => x.StartDate, f => f.Date.Recent(7)).
                                           RuleFor(x => x.EndDate, f => f.Date.Soon(31)).
                                           RuleFor(x => x.InvoiceDate, f => f.Date.Future(45)).
                                           RuleFor(x => x.OrderStatus, f => (OrderStatus) f.Random.Int(0, 3));

            orderList.AddRange(Enumerable.Range(0, 10).Select(_ => order.Generate()).ToArray());

            order = new Faker<Order>().RuleFor(x => x.ObjectNumber, _ => $"B-{orderIds++}").
                                       RuleFor(x => x.Address, f => f.Person.Address.Street).
                                       RuleFor(x => x.Description, f => f.Lorem.Sentence(7)).
                                       RuleFor(x => x.StartDate, f => f.Date.Soon(7)).
                                       RuleFor(x => x.OrderStatus, _ => OrderStatus.NotStarted);

            orderList.AddRange(Enumerable.Range(0, 10).Select(x => order.Generate()).ToArray());

            dbContext.Orders.AddRange(orderList);

            return orderList;
        }

        private static IReadOnlyCollection<Worker> SeedWorkerTable(AppDbContext dbContext)
        {
            var worker = new Faker<Worker>()
                        .RuleFor(x => x.Name, f => f.Person.FullName)
                        .RuleFor(x => x.Email, f => f.Person.Email)
                        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("####-######"));

            var workers = Enumerable.Range(0, 20).Select(_ => worker.Generate()).ToArray();

            dbContext.Workers.AddRange(workers);

            return workers;
        }

        private static void SeedExpenseTable(
            AppDbContext dbContext,
            IReadOnlyCollection<Order> orders,
            IReadOnlyCollection<Worker> workers)
        {
            var fakeExpense = new Faker<Expense>()
                              .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                              .RuleFor(x => x.Price, f => double.Parse(f.Commerce.Price()))
                              .RuleFor(x => x.Product, f => (Product)f.Random.Int(0, 3));

            for(var i = 0; i < orders.Count; i++)
            {
                var expenses = Enumerable.Range(0, 5).Select(_ => fakeExpense.Generate()).ToArray();
                expenses.ForAll(x => x.Order = orders.ElementAt(i));
                expenses.ForAll(x => x.Worker = workers.ElementAt(i));

                dbContext.Expenses.AddRange(expenses);
            }
        }

        private static void SeedTimeRegistrationTable(
            AppDbContext dbContext,
            IReadOnlyCollection<Order> orders,
            IReadOnlyCollection<Worker> workers)
        {
            var fakeTimeRegistration = new Faker<TimeRegistration>()
                              .RuleFor(x => x.Week, f =>
                              {
                                  var date = f.Date.Recent(14);
                                  return $"{ISOWeek.GetYear(date)}{ISOWeek.GetWeekOfYear(date)}";
                              })
                              .RuleFor(x => x.DateTime, f => DateTime.UtcNow.AddDays(f.Random.Int(0, 14)))
                              .RuleFor(x => x.Hours, f =>
                              {
                                  var d = f.Random.Double(1, 40);
                                  return Math.Round(d * 2, MidpointRounding.AwayFromZero) / 2;
                              });

            for(var i = 0; i < orders.Count; i++)
            {
                var timeRegistration = fakeTimeRegistration.Generate();
                timeRegistration.Order = orders.ElementAt(i);
                timeRegistration.Worker = workers.ElementAt(i);
                dbContext.TimeRegistrations.Add(timeRegistration);

                var timeRegistrations = Enumerable.Range(0, 5).Select(_ => fakeTimeRegistration.Generate()).ToArray();
                timeRegistrations.ForAll(x => x.Order = orders.ElementAt(i));
                timeRegistrations.ForAll(x => x.Worker = workers.ElementAt(i));

                dbContext.TimeRegistrations.AddRange(timeRegistrations);
            }
        }
    }
}
