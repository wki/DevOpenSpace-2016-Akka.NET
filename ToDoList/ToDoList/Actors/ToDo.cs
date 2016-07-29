using System;
using Akka.Actor;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    public class ToDo : DurableActor
    {
        public string Id { get; set; }
        public string Description { get; set; }

        protected override string PersistenceId { get; set; }

        public ToDo(string id)
        {
            Id = id;
            PersistenceId = String.Format("Todo-{0}", id);

            // Kommandos
            Command<SpecifyToDo>(s => SpecifyToDo(s));
            Command<MoveUpToDo>(m => MoveUpToDo(m));

            // Ereignisse
            Recover<ToDoSpecified>(s => ToDoSpecified(s));
            Recover<ToDoMovedUp>(m => ToDoMovedUp(m));
        }

        // Kommando Behandlung: Validierung und Event Persistierung
        private void SpecifyToDo(SpecifyToDo specifyToDo)
        {
            Context.System.Log.Debug("Actor {0}: received command {1}", 
                Self.Path.Name, specifyToDo
            );

            Persist(new ToDoSpecified(specifyToDo.Id, specifyToDo.Description));
        }

        private void MoveUpToDo(MoveUpToDo moveUpToDo)
        {
            Context.System.Log.Debug("Actor {0}: received command {1}", 
                Self.Path.Name, moveUpToDo
            );

            Persist(new ToDoMovedUp(moveUpToDo.Id));
        }

        // Event Behandlung: interner Zustand verändert
        private void ToDoSpecified(ToDoSpecified toDoSpecified)
        {
            Context.System.Log.Debug("Actor {0}: received event {1}", 
                Self.Path.Name, toDoSpecified
            );

            Id = toDoSpecified.Id;
            Description = toDoSpecified.Description;
        }

        private void ToDoMovedUp(ToDoMovedUp toDoMovedUp)
        {
            Context.System.Log.Debug("Actor {0}: received event {1}", 
                Self.Path.Name, toDoMovedUp
            );
 
            // hier passiert nichts, nur im View behandeln wir das.
        }
    }
}
