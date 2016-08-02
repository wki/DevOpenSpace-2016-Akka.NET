using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using DeployActors.Actors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace DeployActors
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new AppOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Port > 0)
                    StartService(options.Port);
                else
                    StartMaster();
            }
        }

        private static void StartService(int port)
        {
            Console.WriteLine($"Starting service on Port {port}");

            var config = ConfigurationFactory.ParseString($"akka.remote.helios.tcp.port = {port}")
                .WithFallback(ConfigurationFactory.Load());
            var system = ActorSystem.Create("Service", config);

            // "Service" startet keine Aktoren.
            // Das passiert via Deployment vom "Master" aus

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Terminate().Wait();
        }

        private static void StartMaster()
        {
            Console.WriteLine("Starting master");

            var config = ConfigurationFactory.Load();
            var system = ActorSystem.Create("Master", config);

            // Deployen der Aktoren Als Name verwenden wir "Inc" + port
            for (var port = 8081; port <= 8084; port++)
                system.ActorOf(
                    Props.Create<Increment>(),
                    $"Inc{port}"
                );

            // über diesen Router sprechen wir unsere Aktoren an
            // der Router verteilt die Zugriffe entsprechend
            var inc = system.ActorOf(
                Props.Create<Increment>()
                     .WithRouter(FromConfig.Instance),
                "IncTc"
            );

            var random = new Random();

            while (true)
            {
                for (var i = 0; i < 10; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    var number = random.Next(1, 100);
                    Console.WriteLine($"Asking number={number}...");
                    var result = inc.Ask<int>(number).Result;

                    var elapsed = stopwatch.Elapsed.TotalMilliseconds;
                    Console.WriteLine($"Got Result: {result} ({elapsed:F1}ms)");
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.WriteLine("Press [enter] to repeat, Ctrl-C to stop");
                Console.ReadLine();
            }

            system.Terminate().Wait();
        }
    }
}
