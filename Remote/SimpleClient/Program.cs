using System;
using Akka.Actor;
using Akka.Configuration;

namespace SimpleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Client", ConfigurationFactory.Load());

            var senderActor = system.ActorOf(Props.Create<SenderActor>(), "sender");
            // so klappt die Antwort nicht -- was könnte der Grund sein?
            //system.ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
            //      .Tell("Hi there");

            // Alternative 1:
            system.ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
                  .Tell("Hi there", senderActor);

            Console.WriteLine("Press [enter] to stop");
            Console.ReadLine();

            system.Terminate().Wait();
        }
    }

    public class SenderActor : ReceiveActor
    {
        public SenderActor()
        {
            Receive<string>(s =>
                Console.WriteLine($"Received '{s}' from {Sender}")
            );
        }

        protected override void PreStart()
        {
            base.PreStart();

            // Alternative 2:
            //Context.System
            //    .ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
            //    .Tell("Hi there");
        }
    }
}
