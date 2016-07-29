using Akka.Actor;
using System;
using ToDoList.Messages;
using System.Collections.Generic;

namespace ToDoList.Actors
{
    public abstract class DurableActor : DurableBase
    {
        // must be defined by inheriting class
        protected abstract string PersistenceId { get; set; }
        private readonly IActorRef journalWriter;

        protected DurableActor()
        {
            journalWriter = Context.ActorOf(Props.Create<JournalWriter>(), "writer");

            // wir müssen uns das mitteilen, damit der übergeordnete Konstruktor
            // selbst einen Zustand einstellen kann.
            Self.Tell(new RestoreFromJournal("*-" + PersistenceId));
        }

        protected void Command<C>(Action<C> commandHandler)
        {
            commands.Add(new Handler(typeof(C), c => commandHandler((C)c)));
        }

        protected void Persist(object message)
        {
            Context.System.Log.Debug("Actor {0}: Persist {1}", Self.Path.Name, message);

            journalWriter.Tell(
                new PersistToJournal(PersistenceId, message)
            );
        }
    }
}
