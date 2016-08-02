using Akka.Actor;
using System;

namespace DeployActors.Actors
{
    class Increment : ReceiveActor
    {
        public Increment()
        {
            Receive<int>(i => IncrementNumber(i));
        }

        private void IncrementNumber(int i)
        {
            Console.WriteLine($"{Self.Path} Received Increment Request {i} from {Sender.Path}");

            Sender.Tell(i + 1);
        }
    }
}
