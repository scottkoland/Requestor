
using MassTransit;
using Topshelf;
using System;
using Core;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.SetServiceName("TestService");
                c.SetDisplayName("TestService");
                c.SetDescription("");

                c.RunAsLocalSystem();
                c.DependsOnMsmq();

                c.Service<WorkerService>(s =>
                {
                    s.ConstructUsing(builder => new WorkerService());
                    s.WhenStarted(o => o.Start());
                    s.WhenStopped(o => o.Stop());
                });
            });
        }
    }

    internal class Consumer : Consumes<YourMessage>.All
    {
        public void Consume(YourMessage message)
        {
            Console.WriteLine("Got message " + message.Text);
        }
    }

    internal class WorkerService
    {
        private IServiceBus bus;

        public WorkerService()
        {
            bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                //sbc.VerifyMsmqConfiguration();
                sbc.UseMulticastSubscriptionClient();
                sbc.ReceiveFrom("msmq://localhost/test_queue_responder");
                sbc.SetConcurrentConsumerLimit(1);
                sbc.UseControlBus();

                sbc.Subscribe(s => s.Consumer<Consumer>());
                sbc.Subscribe(subs =>
                                  {
                                      subs.Handler<YourMessage>(msg => Console.WriteLine(msg.Text));
                                  });
            });
        }

        public void Start()
        {
            Console.WriteLine("Start");
        }
        public void Stop()
        {
            Console.WriteLine("Stop");
        }
    }
}
