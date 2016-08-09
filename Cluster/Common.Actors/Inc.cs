using Akka.Actor;

namespace Common.Actors
{
    public class Inc: ReceiveActor
    {
        public Inc()
        {
            Receive<int>(i => Sender.Tell(i + 1));
        }
    }
}
