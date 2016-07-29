using System;
using System.Linq;
using Akka.Actor;
using System.Threading;

namespace PiCalculator
{
    /// <summary>
    /// Ein Worker errechnet die Summe über einen gegebenen Bereich
    /// </summary>
    public class Worker : ReceiveActor
    {
        /// <summary>
        /// Kommando für die Berechnung
        /// </summary>
        public class CalculateRange
        {
            public int From { get; private set; }
            public int To { get; private set; }

            public CalculateRange(int from, int to)
            {
                From = from;
                To = to;
            }
        }

        public Worker()
        {
            Console.WriteLine("  Starting Worker {0}", Self.Path.Name);

            Receive<CalculateRange>(r => CalculateRangeHandler(r));
        }

        /// <summary>
        /// Behandlung des Kommandos
        /// </summary>
        /// <param name="range">Range.</param>
        private void CalculateRangeHandler(CalculateRange range)
        {
            Console.WriteLine("  {0}: Calculating Range from {1} to {2} Thread {3}", 
                Self.Path.Name, range.From, range.To, Thread.CurrentThread.ManagedThreadId
            );

            // rechnen
            double sum = 0;
            var factor = Math.Pow(-1, range.From - 1);
            int denom = 2 * range.From - 1;
            for (int k = range.From; k <= range.To; k++)
                sum += (factor *= -1) / (denom += 2);

            // und zurückschicken
            Sender.Tell(sum * 4);
        }
    }
}
