using System;
using Akka.Actor;
using Akka.Configuration;

namespace SimpleServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Server", ConfigurationFactory.Load());

            var helloActor = system.ActorOf(Props.Create<HelloActor>(), "hello");

            Console.WriteLine("Waiting for remote calls. Press [enter] to stop");
            Console.ReadLine();

            system.Terminate().Wait();
        }
    }

    public class HelloActor : ReceiveActor
    {
        public HelloActor()
        {
            Receive<string>(s =>
                {
                    Console.WriteLine($"String '{s}' received from {Sender}");
                    Sender.Tell($"Answer to '{s}'");
                });
        }
    }
}
