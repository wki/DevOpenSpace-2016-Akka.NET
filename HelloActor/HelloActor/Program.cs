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

            // TODO: Nachrichten von andere Datentypen an den Actor senden

            // TODO: zweiten HelloActor (anderer Name!) anlegen

            // TODO: wechselweise beiden Aktoren Nachrichten senden

            // Gotcha: Aktor mit gleichem Namen anlegen

            // TODO: Aktor Namen des Senders und Empfängers mit ausgeben (Hinweis: Context)

            Console.WriteLine("Press [enter] to stop");
            Console.ReadLine();

            system.Terminate().Wait();
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
