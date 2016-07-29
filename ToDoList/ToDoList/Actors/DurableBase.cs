using System;
using System.Collections.Generic;
using Akka.Actor;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    public abstract class DurableBase : UntypedActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        protected List<Handler> commands;
        protected List<Handler> events;

        protected IActorRef journalReader;

        protected DurableBase()
        {
            commands = new List<Handler>();
            events = new List<Handler>();
            journalReader = Context.ActorOf(Props.Create<JournalReader>(), "reader");
        }

        // inheriting class uses it to declare events
        protected void Recover<E>(Action<E> eventHandler)
        {
            events.Add(new Handler(typeof(E), e => eventHandler((E)e)));
        }


        protected override void OnReceive(object message)
        {
            if (message is RestoreFromJournal)
            {
                Context.System.Log.Debug("Actor {0}: RestoreFromJournal", Self.Path.Name);

                BecomeStacked(Restoring);
                journalReader.Tell(message);
            }
            else
            {
                var handler = events.Find(e => e.Type == message.GetType())
                    ?? commands.Find(c => c.Type == message.GetType());
                if (handler != null)
                    handler.Action(message);
                else
                    Unhandled(message);
            }
        }

        private void Restoring(object message)
        {
            if (message is CompletedRestore)
            {
                Context.System.Log.Debug("Actor {0}: CompletedRestore", Self.Path.Name);

                UnbecomeStacked();
                Stash.UnstashAll();
            }
            else
            {
                var eventHandler = events.Find(e => e.Type == message.GetType());
                if (eventHandler != null)
                {
                    Context.System.Log.Debug("Actor {0}: Restore Event: {1}", Self.Path.Name, message);

                    eventHandler.Action(message);
                }
                else
                {
                    Context.System.Log.Debug("Actor {0} during restore stash command: {1}", Self.Path.Name, message);

                    Stash.Stash();
                }
            }
        }
    }
}
