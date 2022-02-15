using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, bool forceSeed = false)
        {
            using var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
            SeedDatabase(dbContext, forceSeed);
        }

        public static void SeedDatabase(AppDbContext dbContext, bool forceSeed)
        {
            dbContext.Database.Migrate();

            var alreadySeeded = CheckIfDatabaseIsAlreadySeeded(dbContext);
            if (alreadySeeded && forceSeed == false)
            {
                return;
            }
            
            ClearAllTables(dbContext);
            SeedTables(dbContext);
            dbContext.SaveChanges();
        }

        private static void ClearAllTables(AppDbContext db)
        {
            db.TimeRegistrations.Clear();
            db.Expenses.Clear();
            db.Products.Clear();
            db.Orders.Clear();
            db.Workers.Clear();
            db.SaveChanges();
        }

        private static void SeedTables(AppDbContext dbContext)
        {
            var orders = SeedOrderTable(dbContext);
            var workers = SeedWorkerTable(dbContext);
            var productIds = SeedProductTable(dbContext);
            SeedExpenseTable(dbContext, orders, workers, productIds);
            SeedTimeRegistrationTable(dbContext, orders, workers);
        }

        private static bool CheckIfDatabaseIsAlreadySeeded(AppDbContext dbContext)
        {
            return dbContext.Orders.Any() &&
                   dbContext.TimeRegistrations.Any() &&
                   dbContext.Expenses.Any() &&
                   dbContext.Workers.Any();
        }

        private static IReadOnlyCollection<Order> SeedOrderTable(AppDbContext dbContext)
        {
            var orderIds = 900;
            List<Order> orderList = new();
            var order = new Faker<Order>().RuleFor(x => x.ObjectNumber, _ => $"B-{orderIds++}").
                                           RuleFor(x => x.Address, f => f.Person.Address.Street).
                                           RuleFor(x => x.Description, f => f.Lorem.Sentence(7)).
                                           RuleFor(x => x.CustomerName, f => f.Company.CompanyName()).
                                           RuleFor(x => x.CustomerPhoneNumber, f => f.Phone.PhoneNumber("####-######")).
                                           RuleFor(x => x.StartDate, f => f.Date.Recent(7)).
                                           RuleFor(x => x.EndDate, f => f.Date.Soon(31)).
                                           RuleFor(x => x.InvoiceDate, f => f.Date.Future(45)).
                                           RuleFor(x => x.OrderStatus, f => (OrderStatus)f.Random.Int(0, 3));

            orderList.AddRange(Enumerable.Range(0, 10).Select(_ => order.Generate()).ToArray());

            order = new Faker<Order>().RuleFor(x => x.ObjectNumber, _ => $"B-{orderIds++}").
                                       RuleFor(x => x.Address, f => f.Person.Address.Street).
                                       RuleFor(x => x.Description, f => f.Lorem.Sentence(7)).
                                       RuleFor(x => x.CustomerName, f => f.Company.CompanyName()).
                                       RuleFor(x => x.CustomerPhoneNumber, f => f.Phone.PhoneNumber("####-######")).
                                       RuleFor(x => x.StartDate, f => f.Date.Soon(7)).
                                       RuleFor(x => x.OrderStatus, _ => OrderStatus.NotStarted);

            orderList.AddRange(Enumerable.Range(0, 10).Select(x => order.Generate()).ToArray());

            dbContext.Orders.AddRange(orderList);

            dbContext.SaveChanges();

            return orderList;
        }

        private static IReadOnlyCollection<Worker> SeedWorkerTable(AppDbContext dbContext)
        {
            var worker = new Faker<Worker>().RuleFor(x => x.Name, f => f.Person.FullName).
                                             RuleFor(x => x.Company, f => f.Company.CompanyName()).
                                             RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("####-######"));

            var workers = Enumerable.Range(0, 20).Select(_ => worker.Generate()).ToArray();

            dbContext.Workers.AddRange(workers);

            dbContext.SaveChanges();

            return workers;
        }

        private static int[] SeedProductTable(AppDbContext dbContext)
        {
            var products = new[]
                {
                    (Type: "Machine", Description: "New or leased machines"),
                    (Type: "Worker clothes", Description: "All clothing and accessories for workers"),
                    (Type: "Tools", Description:"All tools that are not considered machines (i.e. hammer, screwdriver etc)"),
                    (Type: "Material", Description:"All material (e.g. nails, paint, wood etc)"),
                }.Select(
                      x => new Product
                      {
                          Type = x.Type,
                          Description = x.Description,
                      }).
                  ToArray();

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            return products.Select(x => x.Id).ToArray();
        }

        private static void SeedExpenseTable(
            AppDbContext dbContext,
            IReadOnlyCollection<Order> orders,
            IReadOnlyCollection<Worker> workers,
            int[] productIds)
        {
            var fakeExpense = new Faker<Expense>().RuleFor(x => x.Description, f => f.Lorem.Sentence()).
                                                   RuleFor(x => x.Price, f => double.Parse(f.Commerce.Price()));

            var rand = new Random();
            for (var i = 0; i < orders.Count; i++)
            {
                var expenses = Enumerable.Range(0, 5).Select(_ => fakeExpense.Generate()).ToList();
                expenses.ForEach(x => x.Order = orders.ElementAt(i));
                expenses.ForEach(x => x.Worker = workers.ElementAt(i));
                expenses.ForEach(x => x.ProductId = productIds[rand.Next(0, productIds.Length - 1)]);

                dbContext.Expenses.AddRange(expenses);
            }
        }

        private static void SeedTimeRegistrationTable(
            AppDbContext dbContext,
            IReadOnlyCollection<Order> orders,
            IReadOnlyCollection<Worker> workers)
        {
            var fakeTimeRegistration = new Faker<TimeRegistration>().RuleFor(
                                                                         x => x.Week,
                                                                         f =>
                                                                         {
                                                                             var date = f.Date.Recent(14);

                                                                             return
                                                                                 $"{ISOWeek.GetYear(date)}{ISOWeek.GetWeekOfYear(date)}";
                                                                         }).
                                                                     RuleFor(
                                                                         x => x.Day,
                                                                         f => DateTime.UtcNow.AddDays(f.Random.Int(0, 14))).
                                                                     RuleFor(
                                                                         x => x.Hours,
                                                                         f =>
                                                                         {
                                                                             var d = f.Random.Double(1, 40);

                                                                             return Math.Round(
                                                                                 d * 2,
                                                                                 MidpointRounding.AwayFromZero) /
                                                                             2;
                                                                         });

            for (var i = 0; i < orders.Count; i++)
            {
                var timeRegistration = fakeTimeRegistration.Generate();
                timeRegistration.Order = orders.ElementAt(i);
                timeRegistration.Worker = workers.ElementAt(i);
                dbContext.TimeRegistrations.Add(timeRegistration);

                var timeRegistrations = Enumerable.Range(0, 5).Select(_ => fakeTimeRegistration.Generate()).ToList();
                timeRegistrations.ForEach(x => x.Order = orders.ElementAt(i));
                timeRegistrations.ForEach(x => x.Worker = workers.ElementAt(i));

                dbContext.TimeRegistrations.AddRange(timeRegistrations);
            }
        }
    }
}