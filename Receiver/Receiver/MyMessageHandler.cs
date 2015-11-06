using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Receiver
{
    class MyMessageHandler : IHandleMessages<SampleMessage>
    {
        readonly Statistics _stats;

        public MyMessageHandler(Statistics stats)
        {
            this._stats = stats;
        }

        public void Handle(SampleMessage message)
        {
            Program.metric.Mark();
            _stats.MessageProcessed();
        }
    }
}