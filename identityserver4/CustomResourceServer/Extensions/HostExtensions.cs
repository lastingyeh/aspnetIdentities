using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomResourceServer.Data;
using CustomResourceServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomResourceServer.Extensions
{
    public static class HostExtensions
    {
        public static async Task MigrateAndSeedAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataEventRecordContext>();

            await context.Database.MigrateAsync();

            if (!context.DataEventRecords.Any())
            {
                var dataEventRecords = new List<DataEventRecord>
                {
                    new DataEventRecord
                    {
                        Id = Guid.NewGuid(),
                        Name = "eventCreated",
                        Timestamp = DateTime.UtcNow,
                        Description = "Events created"
                    }
                };

                context.DataEventRecords.AddRange(dataEventRecords);

                await context.SaveChangesAsync();
            }
        }
    }
}
