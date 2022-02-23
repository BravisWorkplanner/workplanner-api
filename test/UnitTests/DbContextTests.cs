using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Expenses;
using Domain.Entities;
using Infrastructure.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Xunit;
using AppDbContext = Infrastructure.EF.AppDbContext;

namespace UnitTests
{
    public class DbContextTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public DbContextTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                              .UseSqlite(_connection)
                              .Options;

            using var context = new AppDbContext(_contextOptions);

            context.Database.EnsureCreated();
        }

        private AppDbContext CreateContext() => new(_contextOptions);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public async Task DbContextTransaction_Should_Return_NonNullTransaction()
        {
            var db = CreateContext();

            await db.BeginTransactionAsync();

            Assert.NotNull(db.Database.CurrentTransaction);

            await db.CommitTransactionAsync();

            Assert.Null(db.Database.CurrentTransaction);
        }

        [Fact]
        public async Task DbContextException_Should_Rollback_And_Close_Transaction()
        {
            var db = CreateContext();

            await db.BeginTransactionAsync();

            Assert.NotNull(db.Database.CurrentTransaction);

            db.TimeRegistrations.Add(new TimeRegistration{ OrderId = int.MaxValue});

            try
            {
                await db.CommitTransactionAsync();
            }
            catch
            {
                // we mock a database error by adding time registration and violating foreign key constraint
            }

            Assert.Null(db.Database.CurrentTransaction);
        }
    }
}
