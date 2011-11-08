
using System.Threading;
using MassTransit;
using System;
using Core;

namespace Requestor
{
    public class Program
    {
        public static void Main()
        {
            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                //sbc.VerifyMsmqConfiguration();
                sbc.ReceiveFrom("msmq://localhost/test_queue_requestor");
                sbc.Subscribe(subs =>
                {
                    //subs.Handler<YourMessage>(msg => Console.WriteLine(msg.Text));
                });
            });

            bus.Publish(new YourMessage { Text = "Hi" });

            Thread.Sleep(2000);
        }
    }
}
