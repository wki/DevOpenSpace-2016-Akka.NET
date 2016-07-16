using System;
using Akka.Actor;
using System.Threading;

namespace CrashingHelloActor
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("CrashingHello");

            var supervisor = system.ActorOf(Props.Create<SupervisingActor>(), "supervisor");

            // Alternativ:
            // var supervisor = system.ActorOf(
            //     Props.Create<SupervisingActor>()
            //         .WithSupervisorStrategy(
            //             new OneForOneStrategy(exception => Directive.Restart)
            //         ), 
            //     "supervisor");

            // Egal was wir senden
            supervisor.Tell(42);

            Thread.Sleep(1000);
            Console.WriteLine("Press [enter] to stop");
            Console.ReadLine();

            system.Terminate().Wait();
        }
    }

    public class SupervisingActor : ReceiveActor
    {
        private IActorRef crashingActor;

        public SupervisingActor()
        {
            crashingActor = Context.ActorOf(Props.Create<CrashingActor>(), "crasher");

            Receive<int>(_ => TalkToCrashingActor());
        }

        private void TalkToCrashingActor()
        {
            Console.WriteLine("Talking to Crasher");

            for (var i = 1; i < 100; i++)
                crashingActor.Tell(i);
        }

        // TODO: mit der SupervisorStrategy experimentieren
//        protected override SupervisorStrategy SupervisorStrategy()
//        {
//            // immer die gleiche Entscheidung
//            return new OneForOneStrategy(
//                maxNrOfRetries: 10, 
//                withinTimeRange: TimeSpan.FromSeconds(30), 
//                localOnlyDecider: exception => 
//                { 
//                    Console.WriteLine("*** Supervision: Restart");
//                    return Directive.Restart; 
//                }
//            );
//
//            // wenn wir etwas mehr Logik benötigen:
//            // return new OneForOneStrategy( //or AllForOneStrategy
//            //     maxNrOfRetries: 10,
//            //     withinTimeRange: TimeSpan.FromSeconds(30),
//            //     decider: Decider.From(x =>
//            //          {
//            //              // Beispiele für Verhalten
//            //              if (x is ArithmeticException) 
//            //              {
//            //                  Console.WriteLine("*** Supervision: Resume");
//            //                  return Directive.Resume;
//            //              }
//            //      
//            //              //Error that we cannot recover from, stop the failing actor
//            //              else if (x is NotSupportedException)
//            //              {
//            //                  Console.WriteLine("*** Supervision: Stop");
//            //                  return Directive.Stop;
//            //              }
//            //      
//            //              //In all other cases, just restart the failing actor
//            //              else 
//            //              {
//            //                  Console.WriteLine("*** Supervision: Restart");
//            //                  return Directive.Restart;
//            //              }
//            //         }
//            //     )
//            // );
//        }
    }

    public class CrashingActor : ReceiveActor
    {
        private Random random;

        public CrashingActor()
        {
            random = new Random();

            Receive<int>(i => WriteMessage(i));
        }

        // TODO: Implementieren von PreRestart / PostRestart
        // TODO: bei PreRestart kann die letzte Nachricht nochmals gesendet werden.

        private void WriteMessage(int i)
        {
            // simulieren eines Crashs
            if (random.Next(10) < 1)
                throw new ArgumentException("invalid foo detected");

            Console.WriteLine("Received: {0}", i);
        }
    }
}
