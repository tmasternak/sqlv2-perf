using System;
using System.Threading;
using Messages;
using Metrics;
using NServiceBus;
using NServiceBus.Features;

namespace Sender
{
    class Program
    {
        static readonly Meter writerMetric = Metric.Meter("Writer", Unit.Items);
        static void Main(string[] args)
        {
            Messages.Metrics.Init();
            var cfg = new BusConfiguration();

            cfg.UsePersistence<InMemoryPersistence>();
            cfg.UseSerialization<JsonSerializer>();
            cfg.UseTransport<SqlServerTransport>();
            cfg.DisableFeature<Audit>();
            cfg.Transactions().DisableDistributedTransactions();
            cfg.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            using (var bus = Bus.Create(cfg))
            {
                SpawnWriters(bus, 10, 1000);

                Console.ReadKey();
            }
        }

        private static void SpawnWriters(IBus bus, int threads, int snapshotInterval)
        {
            var stats = new Statistics("Write", snapshotInterval);

            stats.Start();

            for (int i = 0; i < threads; i++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        bus.Send(new SampleMessage());
                        writerMetric.Mark();
                        stats.MessageProcessed();
                    }
                }).Start();
            }
        }
    }
}
