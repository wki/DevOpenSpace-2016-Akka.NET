using System;
using Akka.Actor;
using MapReduce.Messages;
using System.Threading;

namespace MapReduce.Actors
{
    public class Aggregator : ReceiveActor
    {
        private TotalResult totalResult;

        public Aggregator()
        {
            totalResult = new TotalResult();

            Receive<ReduceResult>(r => Aggregate(r));
            Receive<GetResult>(_ => Sender.Tell(totalResult));
        }

        private void Aggregate(ReduceResult result)
        {
            foreach (var key in result.Result.Keys)
            {
                if (totalResult.Result.ContainsKey(key))
                {
                    totalResult.Result[key] += result.Result[key];
                }
                else
                {
                    totalResult.Result[key] = result.Result[key];
                }
            }

            Console.WriteLine("Aggregator: {0}", totalResult);

            // simulate some runtime...
            // Thread.Sleep(50);

            Sender.Tell(totalResult);
        }
    }
}
