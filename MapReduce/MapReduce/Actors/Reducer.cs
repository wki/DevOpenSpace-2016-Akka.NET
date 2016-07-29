using System;
using Akka.Actor;
using MapReduce.Messages;
using System.Threading;

namespace MapReduce.Actors
{
    public class Reducer : ReceiveActor
    {
        public Reducer()
        {
            Receive<MapResult>(m => Reduce(m));
        }

        private void Reduce(MapResult mapResult)
        {
            var reduceResult = new ReduceResult();

            foreach (var count in mapResult.Counts)
            {
                if (reduceResult.Result.ContainsKey(count.Key))
                {
                    reduceResult.Result[count.Key] += count.Count;
                }
                else
                {
                    reduceResult.Result[count.Key] = count.Count;
                }
            }

            Console.WriteLine("Reducer [{0}/{1}]: {2}", 
                Self.Path.Name, Thread.CurrentThread.ManagedThreadId, reduceResult);

            // simulate some runtime...
            Thread.Sleep(50);

            Sender.Tell(reduceResult);
        }
    }
}
