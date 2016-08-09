using Akka.Actor;
using Akka.Configuration;
using System;
using Monitor.Actors;

namespace Monitor
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Monitor Node");

            var system = ActorSystem.Create("LoremIpsum", ConfigurationFactory.Load());

            system.ActorOf(Props.Create<MonitorActor>(), "monitor");

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Terminate().Wait();
        }

    }
}
