using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    /// <summary>
    /// Dieser Aktor steuert das gesamte Spiel
    /// </summary>
    public class GuessGame : ReceiveActor
    {
        private IActorRef chooser;
        private IActorRef enquirer;
        private IActorRef starter;

        // bei Tests mocken wir die diversen Kommunikationspartner
        public GuessGame(IActorRef chooser, IActorRef enquirer, IActorRef starter)
        {
            this.chooser = chooser;
            this.enquirer = enquirer;
            this.starter = starter;

            HandleReceiveMessages();
        }

        public GuessGame()
        {
            chooser = Context.ActorOf(Props.Create<Chooser>(), "Chooser");
            enquirer = Context.ActorOf(Props.Create<Enquirer>(Self, chooser), "Enquirer");

            HandleReceiveMessages();
        }

        private void HandleReceiveMessages()
        {
            Receive<Start>(_ => StartGame());
            Receive<Started>(_ => StartGuessing());
            Receive<Guessed>(guessed => EndGame(guessed));
        }

        private void StartGame()
        {
            Console.WriteLine("Game: We are told to start the game");
            starter = Sender;
            chooser.Tell(new Start());
        }

        private void StartGuessing()
        {
            Console.WriteLine("Game: Now we can start guessing");
            enquirer.Tell(new Start());
        }

        private void EndGame(Guessed guessed)
        {
            Console.WriteLine("Game: Number is guessed, game is over");

            starter.Tell(guessed);
        }
    }
}
