using System;
using Akka.Actor;
using System.Collections.Generic;

namespace PiCalculator
{
    /// <summary>
    /// manuelle Verteilung der Aufgaben an Rechen Aktoren (Worker)
    /// durch einen zwischen geschalteten Router
    /// </summary>
    public class Master : ReceiveActor
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

        // Kumulierung der Summe erfolgt hier
        private double sum;

        // mitzählen wieviele Antworten noch fehlen
        private int nrMissing;

        // Der Router der an die Worker verteilt
        private IActorRef router;

        // Der Initiator, dem wir das Ergebnis am Ende melden
        private IActorRef creator;

        public Master(int nrWorkers)
        {
            Console.WriteLine("Starting Master {0}", Self.Path.Name);

            router = Context.ActorOf(Props.Create<Router>(nrWorkers));

            Receive<CalculatePi>(c => CalculatePiHandler(c));
            Receive<double>(d => SumHandler(d));
        }

        /// <summary>
        /// Kommando Behandlung für CalculatePi
        /// </summary>
        /// <param name="calculatePi">Calculate Pi command</param>
        private void CalculatePiHandler(CalculatePi calculatePi)
        {
            // wer hat die Berechung ursprünglich angefordert?
            creator = Sender;

            // Initialwert der Summe
            sum = 0;

            // mit zählen nicht vergessen
            nrMissing = calculatePi.NrUnits;

            // der Router kümmert sich um die Verteilung
            foreach (var calculateRange in Distribute(calculatePi))
                router.Tell(calculateRange);
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
                Console.WriteLine("All Workers terminated. Pi = {0}", sum);

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
