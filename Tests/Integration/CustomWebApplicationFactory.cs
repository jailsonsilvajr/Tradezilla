﻿using DatabaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                services.AddDbContext<TradezillaContext>(options =>
                    options.UseSqlite(_connection));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TradezillaContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
