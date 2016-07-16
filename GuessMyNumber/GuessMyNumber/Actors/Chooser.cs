using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    /// <summary>
    /// Dieser Aktor sucht sich eine Zahl aus und beantwortet Fragen
    /// </summary>
    public class Chooser : ReceiveActor
    {
        public Random generator;
        public int mySecretNumber;

        public Chooser()
        {
            generator = new Random();

            HandleReceiveMessages();
        }

        public Chooser(Random random)
        {
            generator = random;

            HandleReceiveMessages();
        }


        private void HandleReceiveMessages()
        {
            Receive<Start>(_ => ChooseNumber());
            Receive<TestTry>(t => HandleTestTry(t));
        }

        private void ChooseNumber()
        {
            mySecretNumber = generator.Next(1, 101);

            Console.WriteLine("Chooser: Pssst -- I have something to guess: {0}", mySecretNumber);

            // wir melden dem Spiel zurück, dass wir bereit zum Raten sind
            Sender.Tell(new Started());
        }

        private void HandleTestTry(TestTry testTry)
        {
            var triedNumber = testTry.Number;

            Console.WriteLine("Chooser: Received Guess {0}", triedNumber);

            // TODO: an dieser Stelle müssen wir 3 Fälle behandeln:
            // triedNumber zu klein: TryTooSmall melden
            // triedNumber zu groß: TryTooBig melden
            // triedNumber passt: Guesed melden
            //
            // Nachrichten werden als Antwort versandt nach diesem Muster:
            // Sender.Tell(new MessageClass(args));
        }
    }
}
