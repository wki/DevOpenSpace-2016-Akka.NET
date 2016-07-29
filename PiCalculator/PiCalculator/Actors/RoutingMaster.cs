using System;
using Akka.Actor;
using Akka.Routing;
using System.Collections.Generic;

namespace PiCalculator
{
    /// <summary>
    /// automatische Verteilung der Aufgaben an Rechen Aktoren (Worker)
    /// </summary>
    public class RoutingMaster : ReceiveActor
    {
        /// <summary>
        /// Kommando Nachricht an diesen Aktor
        /// </summary>
        public class CalculatePi
        {
            public int NrUnits { get; private set; }
            public int Series { get; private set; }

            public CalculatePi(int nrUnits, int series)
            {
                NrUnits = nrUnits;
                Series = series;
            }
        }

        // Anzahl der Rechen Aktoren
        private int nrWorkers;

        // Kumulierung der Summe erfolgt hier
        private double sum;

        // mitzählen wieviele Antworten noch fehlen
        private int nrMissing;

        // unser Router, der sich um die Verteilung kümmert
        private IActorRef worker;

        // Der Initiator, dem wir das Ergebnis am Ende melden
        private IActorRef creator;

        /// <summary>
        /// Master Actor for calculating Pi. delegates to workers.
        /// </summary>
        public RoutingMaster(int nrWorkers)
        {
            Console.WriteLine("Starting Routing Master {0}", Self.Path.Name);

            this.nrWorkers = nrWorkers;

            worker = Context.ActorOf(
                Props.Create<Worker>()
                     .WithRouter(new RoundRobinPool(nrWorkers))
            );

            Receive<CalculatePi>(c => CalculatePiHandler(c));
            Receive<double>(d => SumHandler(d));
        }

        /// <summary>
        /// Kommando Behandlung für CalculatePi
        /// </summary>
        /// <param name="calculatePi">Calculate Pi command</param>
        private void CalculatePiHandler(CalculatePi calculatePi)
        {
            // wer hat die Berechung angefordert?
            creator = Sender;

            // Initialwert der Summe
            sum = 0;

            // mit zählen nicht vergessen
            nrMissing = calculatePi.NrUnits;

            // lassen wir verteilt rechnen
            foreach (var calculateRange in Distribute(calculatePi))
                worker.Tell(calculateRange);
        }

        /// <summary>
        /// Behandlung des Summen-Anteils eines Rechen Aktors
        /// </summary>
        /// <param name="s">Sum value</param>
        private void SumHandler(double s)
        {
            Console.WriteLine("Received sum from worker {0}", Sender.Path.Name);
            sum += s;

            if (--nrMissing == 0)
            {
                Console.WriteLine("All Workers finished. Pi = {0}", sum);

                creator.Tell(sum);
            }
        }

        /// <summary>
        /// Kapselung der Erzeugung der Serie in eine Anzahl von gleichverteilten Einheiten
        /// </summary>
        /// <param name="calculatePi">Calculate pi.</param>

        private IEnumerable<Worker.CalculateRange> Distribute(CalculatePi calculatePi)
        {
            int rangeStartAt = 0;
            int nrPartsRemaining = calculatePi.NrUnits;

            while (nrPartsRemaining > 0)
            {
                int rangeEndAt = rangeStartAt + (calculatePi.Series - rangeStartAt) / nrPartsRemaining;

                yield return new Worker.CalculateRange(rangeStartAt, rangeEndAt);

                rangeStartAt = rangeEndAt + 1;
                nrPartsRemaining--;
            }
        }
    }
}
