using Akka.Actor;
using Akka.Routing;
using MapReduce.Actors;
using MapReduce.Messages;
using System.Diagnostics;

namespace MapReduce.Actors
{
    public class Master : ReceiveActor
    {
        const int NrMappers = 3;
        const int NrReducers = 3;

        private readonly IActorRef mapper;
        private readonly IActorRef reducer;
        private readonly IActorRef aggregator;

        private int nrUnitsToDo;
        private int nrUnitsDone;

        private Stopwatch stopwatch;

        public Master()
        {
            stopwatch = Stopwatch.StartNew();

            mapper = Context.ActorOf(
                Props.Create<Mapper>()
                     .WithRouter(new RoundRobinPool(NrMappers))
            );
            reducer = Context.ActorOf(
                Props.Create<Reducer>()
                     .WithRouter(new RoundRobinPool(NrReducers))
            );
            aggregator = Context.ActorOf(Props.Create<Aggregator>());

            nrUnitsToDo = nrUnitsDone = 0;

            // 1. forward a string to mapper
            Receive<string>(s => { nrUnitsToDo++; mapper.Tell(s); });

            // 2. forward map result to reducer
            Receive<MapResult>(m => reducer.Tell(m));

            // 3. forward reduce result to aggregator
            Receive<ReduceResult>(r => aggregator.Tell(r));

            Receive<TotalResult>(t =>
                {
                    if (++nrUnitsDone == nrUnitsToDo)
                        System.Console.WriteLine("*** Done. Final Total Result: {0}, elapsed: {1:N1}s",
                            t, stopwatch.ElapsedMilliseconds / 1000.0
                        );
                });

            // allow asking for aggregated result at any time
            Receive<GetResult>(g => aggregator.Forward(g));
        }
    }
}
