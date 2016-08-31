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

            //system.ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
            //      .Tell("Hi there");

            var senderActor = system.ActorOf(Props.Create<SenderActor>(), "sender");

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
            Context.System.ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
                  .Tell("Hi there");
            Context.System.ActorSelection("akka.tcp://Server@localhost:8000/user/hello")
                  .Tell(4711);

        }
    }
}
