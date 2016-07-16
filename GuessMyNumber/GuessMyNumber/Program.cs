using System;
using Akka.Actor;
using GuessMyNumber.Actors;
using System.Threading;
using GuessMyNumber.Messages;

namespace GuessMyNumber
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Number Guess Starting");

            var system = ActorSystem.Create("Numb3rs");
            var game = system.ActorOf(Props.Create<GuessGame>(), "Game");

            

            game.Tell(new Start());

            // damit sich die Meldungen nicht überschreiben, warten wir bis
            // das Spektakel vorbei ist.
            Thread.Sleep(500);

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Terminate().Wait();
        }
    }
}
