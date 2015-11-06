using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Features;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = new BusConfiguration();

            cfg.UsePersistence<InMemoryPersistence>();
            cfg.UseSerialization<JsonSerializer>();
            cfg.UseTransport<SqlServerTransport>();
            cfg.DisableFeature<Audit>();
            cfg.Transactions().DisableDistributedTransactions();
            cfg.Conventions()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            var stats = new Statistics("Read", 1000);

            stats.Start();

            cfg.RegisterComponents(c => c.RegisterSingleton(stats));

            using (var bus = Bus.Create(cfg).Start())
            {
                Console.Read();
            }
        }
    }
}
