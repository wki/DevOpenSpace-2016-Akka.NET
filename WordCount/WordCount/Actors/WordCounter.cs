using System;
using System.Linq;
using Akka.Actor;
using System.Collections.Generic;
using WordCount.Messages;

namespace WordCount.Actors
{
    /// <summary>
    /// Create a list of words and count their occurence
    /// </summary>
    /// <description>
    /// as long as string messages are received, they are counted as words
    /// As soon as the End message occurs, the count is sent to the next
    /// actor as a sorted list of lines representing the word counts
    /// </description>
	public class WordCounter : ReceiveActor
	{
        private IActorRef next;
        private Dictionary<string, int> wordCount;

        public WordCounter (IActorRef next)
        {
            this.next = next;
            wordCount = new Dictionary<string, int>();

            Receive<string>(s => CountWord(s));
            Receive<End>(end => Terminate(end));
        }

        private void CountWord(string word)
        {
            if (wordCount.ContainsKey(word))
                wordCount[word]++;
            else
                wordCount.Add(word, 1);
        }

        private void Terminate(End end)
        {
            var counts =
                wordCount.Keys
                    .OrderBy(w => w)
                    .Select(w => String.Format("{0}: {1}", w, wordCount[w]));
           
            foreach (var count in counts)
                next.Tell(count);

            next.Tell(end);
        }
	}
}
