using System;
using Akka.Actor;
using System.Collections.Generic;

namespace PiCalculator
{
    /// <summary>
    /// Annahme von Rechenaufgaben und Weiterleitung an Worker
    /// </summary>
    public class Router : ReceiveActor
    {
        // Anzahl der Worker
        private int nrWorkers;

        // Round Robin Pool an Worker Instanzen
        private IList<IActorRef> workers;

        // nächster zu nutzender Worker im Pool
        private int nextWorker;

        public Router(int nrWorkers)
        {
            this.nrWorkers = nrWorkers;
            workers = new List<IActorRef>();
            nextWorker = 0;

            for (var i = 0; i < nrWorkers; i++)
                workers.Add(
                    Context.ActorOf(Props.Create<Worker>())
                );

            // einfach mal alles weiter leiten :-)
            Receive<object>(msg => 
                workers[nextWorker++ % nrWorkers].Forward(msg)
            );
        }
    }
}
