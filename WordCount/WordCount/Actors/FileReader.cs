using System;
using System.IO;
using Akka.Actor;
using System.Text;
using WordCount.Messages;

namespace WordCount.Actors
{
    /// <summary>
    /// Read a file as a sequence of lines
    /// </summary>
    /// <description>
    /// Every read line sends a string message to the next actor in the chain.
    /// If the file end is reached, an End message is sent.
    /// </description>
	public class FileReader : ReceiveActor
	{
        private class ReadNextLine {}

        private IActorRef next;
        private FileStream fileStream;
        private StreamReader streamReader;

        public FileReader(string filePath, IActorRef next)
        {
            this.next = next;

            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            streamReader = new StreamReader(fileStream, Encoding.UTF8);

            Self.Tell(new ReadNextLine());

            Receive<ReadNextLine>(_ => ReadLine());
        }

        private void ReadLine()
        {
            string line = streamReader.ReadLine();

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
