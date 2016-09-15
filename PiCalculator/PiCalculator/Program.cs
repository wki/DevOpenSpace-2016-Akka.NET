using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.Configuration;

namespace PiCalculator
{
    public class MainClass
    {
        // Anzahl der Werte, die wir zur Berechnung verwenden
        private const int Million = 1000 * 1000;
        private const int Series = 800 * Million;

        // Akzahl der Aktoren, die wir gleichzeitig rechnen lassen
        private const int NrWorkersToStart = 4;
         

        // Anzahl der Rechen-Pakete, die wir auf die Aktoren verteilen
        private const int NrUnits = 20;

        public static void Main(string[] args)
        {
            // je nach Inhalt wird eine der 4 Varianten unten ausgeführt.
            const int variant = 2; // 0-3

            var actorSystem = ActorSystem.Create("PiCalculator");

            var stopWatch = Stopwatch.StartNew();


            // SCHLECHTESTE VARIANTE
            // wir haben nur einen Aktor, der rechnet
            if (variant == 0)
                actorSystem.ActorOf(Props.Create<Worker>())
                    .Ask<double>(new Worker.CalculateRange(0, Series))
                    .Wait();

            // BESSER ABER UMSTÄNDLICH
            // wir kümmern uns selbst um das Verteilen
            if (variant == 1)
                actorSystem.ActorOf(Props.Create<Master>(NrWorkersToStart), "MasterWithRouter")
                   .Ask<double>(new Master.CalculatePi(NrUnits, Series))
                   .Wait();


            // EINFACH UND LEISTUNGSFÄHIG
            // dieser Router erzeugt und routet selbst:
            if (variant == 2)
                actorSystem.ActorOf(Props.Create<RoutingMaster>(NrWorkersToStart), "Master" + NrUnits)
                    .Ask<double>(new RoutingMaster.CalculatePi(NrUnits, Series))
                    .Wait();

            // EINFACH, LEISTUNGSFÄHIG UND KONFIGURIERBAR
            // 
            if (variant == 3)
                actorSystem.ActorOf(Props.Create<ConfigRoutingMaster>(), "ConfigRoutingMaster")
                    .Ask<double>(new ConfigRoutingMaster.CalculatePi(NrUnits, Series))
                    .Wait();

            stopWatch.Stop();

            Console.WriteLine("Elapsed: {0:N1}s", stopWatch.ElapsedMilliseconds / 1000.0);
            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();
        }
    }
}
