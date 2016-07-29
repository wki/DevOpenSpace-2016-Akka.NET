using System;
using MapReduce.Messages;
using Akka.Actor;
using System.IO;
using System.Linq;
using System.Threading;

namespace MapReduce.Actors
{
    public class Mapper : ReceiveActor
    {
        public Mapper()
        {
            Receive<string>(s => Map(s));
        }

        private void Map(string input)
        {
            var mapResult = new MapResult();

            using (var reader = new StringReader(input)) 
            {
                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        var key = line.Split(new [] { '|' }).Last().Trim();

                        mapResult.Counts.Add(new KeyCount(key));
                    }
                }
            }

            Console.WriteLine("Mapper [{0}/{1}]: {2}", 
                Self.Path.Name, Thread.CurrentThread.ManagedThreadId, mapResult);

            // simulate some runtime...
            Thread.Sleep(50);

            Sender.Tell(mapResult);
        }
    }
}
