using System;
using Akka.Actor;
using System.Threading.Tasks;
using Akka.Configuration;
using Common.Actors;
using Akka.Routing;

namespace Frontend
{
    public class BackendService : IBackendService
    {
        // wir haben nur einen Service der mit dem Actor Modell spricht
        // daher genügt dieser einfache Ansatz.
        // hätten wir mehr, müssten wir das ActorSystem übergeben.

        // unser Actor System
        private readonly ActorSystem actorSystem;

        // unser "verteilter" Actor
        private readonly IActorRef incrementor;

        /// <summary>
        /// Konstruktor. Eventuell sollten wir das ActorSystem hier übergeben
        /// </summary>
        public BackendService()
        {
            actorSystem = ActorSystem.Create("LoremIpsum", ConfigurationFactory.Load());

            incrementor = actorSystem.ActorOf(
                Props.Create<Inc>().WithRouter(FromConfig.Instance), 
                "inc");
        }

        // wir leiten die "Aufgabe" an den Aktor weiter.
        public Task<int> Increment(int x) => incrementor.Ask<int>(x);
    }
}
