using System;
using Akka.Actor;
using WordCount.Messages;

namespace WordCount.Actors
{
    /// <summary>
    /// Write a string stream as separate lines to the console
    /// </summary>
	public class ConsoleWriter : ReceiveActor
	{
        public ConsoleWriter()
        {
            Receive<string>(s => Console.WriteLine(s));
            // Receive<End>(_ => Context.System.Terminate());
        }
	}
}
