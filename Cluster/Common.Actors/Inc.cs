using Akka.Actor;

namespace Common.Actors
{
    public class Inc: ReceiveActor
    {
        public Inc()
        {
            Receive<int>(i => {
                Context.System.Log.Info($"Received {i} from {Sender}");
                Sender.Tell(i + 1);
            });
        }
    }
}
