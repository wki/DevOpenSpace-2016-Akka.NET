using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    /// <summary>
    /// Dieser Aktor versucht die Zahl zu raten mittels binärer Suche
    /// </summary>
    public class Enquirer : ReceiveActor
    {
        private readonly IActorRef game;
        private readonly IActorRef chooser;

        // possible range of numbers including boundaries
        public int rangeFrom;
        public int rangeTo;

        public Enquirer(IActorRef game, IActorRef chooser)
        {
            Console.WriteLine("Enquirer: Constructor");

            this.chooser = chooser;
            this.game = game;

            rangeFrom = 1;
            rangeTo = 100;

            Receive<Start>(_ => StartGuessing());
            Receive<TryTooSmall>(t => HandleTooSmallTry(t));
            Receive<TryTooBig>(t => HandleTooBigTry(t));
            Receive<Guessed>(g => HandleGuessed(g));
        }

        private void MakeATry()
        {
            var triedNumber = rangeFrom + (rangeTo - rangeFrom) / 2;

            Console.WriteLine("Enquirer: Range {0} - {1}, trying: {2}", rangeFrom, rangeTo, triedNumber);

            chooser.Tell(new TestTry(triedNumber));
        }

        private void StartGuessing()
        {
            Console.WriteLine("Enquirer: Starting to guess");

            MakeATry();
        }

        private void HandleTooSmallTry(TryTooSmall guessTooSmall)
        {
            Console.WriteLine("Enquirer: {0} is too small", guessTooSmall.Number);
            rangeFrom = guessTooSmall.Number + 1;

            MakeATry();
        }

        private void HandleTooBigTry(TryTooBig guessTooBig)
        {
            Console.WriteLine("Enquirer: {0} is too big", guessTooBig.Number);
            rangeTo = guessTooBig.Number - 1;

            MakeATry();
        }

        private void HandleGuessed(Guessed guessed)
        {
            Console.WriteLine("Enquirer: Guessed {0}", guessed.Number);

            game.Tell(guessed);
        }
    }
}
