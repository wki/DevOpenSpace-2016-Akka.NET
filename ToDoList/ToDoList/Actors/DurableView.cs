using System;
using Akka.Actor;
using System.Collections.Generic;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    public abstract class DurableView : DurableBase
    {
        protected DurableView()
        {
            // wir müssen uns das mitteilen, damit der übergeordnete Konstruktor
            // selbst einen Zustand einstellen kann.
            Self.Tell(new RestoreFromJournal("*.json"));

            // grausamer Hack: alle via Broadcast versandten Nachrichten im
            // Namespace "Messages" sind interessant für uns -- das sind in
            // unserem Fall alle Events.
            foreach (var type in typeof(DurableView).Assembly.GetTypes())
            {
                if (type.Namespace.EndsWith(".Messages"))
                    Context.System.EventStream.Subscribe(Self, type);
            }
        }

        // verhält sich wie Command<C> ist aber semantisch klarer.
        protected void Query<C>(Action<C> commandHandler)
        {
            commands.Add(new Handler(typeof(C), c => commandHandler((C)c)));
        }
    }
}
