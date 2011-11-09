
using System.IO;
using System.Threading;
using MassTransit;
using System;
using Core;
using log4net.Config;

namespace Requestor
{
    public class Program
    {
        public static void Main()
        {
            XmlConfigurator.Configure(new FileInfo("requestor.log4net.xml"));

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                sbc.UseMulticastSubscriptionClient(); 
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
