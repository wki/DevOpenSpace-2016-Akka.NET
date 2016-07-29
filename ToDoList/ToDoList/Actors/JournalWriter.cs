using Akka.Actor;
using System;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    public class JournalWriter : JournalActor
    {
        public JournalWriter()
        {
            Receive<PersistToJournal>(p => Persist(p));
        }

        private void Persist(PersistToJournal persistToJournal)
        {
            Context.System.Log.Debug("Actor {0}: Persist to journal", Self.Path.Name);

            var message = persistToJournal.Message;
            var persistenceId = persistToJournal.PersistenceId;
            var type = message.GetType();

            Save(message, type, persistenceId);

            // back to sender to change state
            Sender.Tell(message);

            // allow all views to receive it
            Context.System.EventStream.Publish(message);
        }
    }
}
