using Akka.Actor;
using Akka.Configuration;
using System;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new AppOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var config = ConfigurationFactory.ParseString($"akka.remote.helios.tcp.port = {options.Port}")
                .WithFallback(ConfigurationFactory.Load());
                var system = ActorSystem.Create("LoremIpsum", config);

                // wichtig! wir deployen hier keinen Aktor. Das geschieht über das Frontend

                Console.WriteLine("press [enter] to stop");
                Console.ReadLine();

                system.Terminate().Wait();
            }
        }
    }
}