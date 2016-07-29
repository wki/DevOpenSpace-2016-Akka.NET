using System;
using Akka.Actor;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    public class JournalReader : JournalActor
    {
        public JournalReader()
        {
            Receive<RestoreFromJournal>(r => Restore(r));
        }

        private void Restore(RestoreFromJournal restoreFromJournal)
        {
            Context.System.Log.Debug("Actor {0}: Restore from journal", Self.Path.Name);

            foreach (var filePath in Files(restoreFromJournal.Glob))
                Sender.Tell(Load(filePath));

            Sender.Tell(CompletedRestore.Instance);
        }
    }
}
