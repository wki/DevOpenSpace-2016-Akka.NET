using System;
using Akka.Actor;

namespace HelloActor
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("HelloActorSystem");

            // TODO: Aktor vom Typ "HelloActor" erzeugen
            //       Hinweis: Props.Create<>() als Konstruktor-Ersatz

            // TODO: dem Aktor eine String Nachricht "Hello" senden

            Console.WriteLine("Press [enter] to stop");
            Console.ReadLine();

            system.Terminate();
        }
    }

    public class HelloActor : ReceiveActor
    {
        public HelloActor()
        {
            Receive<string>(message => 
                Console.WriteLine(message)
            );

            Receive<object>(message =>
                Console.WriteLine("OOps: got a {0}: {1}", 
                    message.GetType().FullName, message
                )
            );
        }

        protected override void Unhandled(object message)
        {
            Console.WriteLine("OOps: got a {0}: {1}", 
                message.GetType().FullName, message
            );
        }
    }
}
