using System;
using Akka.Actor;
using System.IO;
using WordCount.Messages;

namespace WordCount.Actors
{
    /// <summary>
    /// Zeilenweise aus String lesen anstelle von Datei
    /// </summary>
    public class TextReader : ReceiveActor
    {
        private class ReadNextLine {}

        private IActorRef next;
        private StringReader stringReader;

        public TextReader(string text, IActorRef next)
        {
            this.next = next;

            stringReader = new StringReader(text);

            Self.Tell(new ReadNextLine());

            Receive<ReadNextLine>(_ => ReadLine());
        }

        private void ReadLine()
        {
            string line = stringReader.ReadLine();

            if (line != null)
            {
                next.Tell(line);
                Self.Tell(new ReadNextLine());
            }
            else
            {
                next.Tell(new End());
            }
        }
    }
}
