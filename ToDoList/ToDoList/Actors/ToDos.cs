using System;
using Akka.Actor;
using ToDoList.Messages;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Actors
{
    /// <summary>
    /// Das "Read Model". Enthält und aktualisiert alle ToDos bei jeder Änderung
    /// </summary>
    public class ToDos : DurableView
    {
        private readonly List<ToDoDocument> toDos;

        public ToDos()
        {
            toDos = new List<ToDoDocument>();

            // Abfragen
            Query<ListToDos>(_ => Sender.Tell(toDos));

            // Ereignisse
            Recover<ToDoSpecified>(s => ToDoSpecified(s));
            Recover<ToDoMovedUp>(m => ToDoMovedUp(m));
        }

        // wir erhalten eine Information über ein neues oder geändertes ToDo
        private void ToDoSpecified(SpecifyToDo toDoSpecified)
        {
            Context.System.Log.Debug("Actor {0}: process {1}", Self.Path.Name, toDoSpecified);

            var todo = new ToDoDocument(toDoSpecified.Id, toDoSpecified.Description);

            var position = toDos.FindIndex(x => x.Id == todo.Id);
            if (position != -1)
                toDos[position] = todo; 
            else
                toDos.Add(todo);

            Context.System.Log.Debug("Actor {0}: TODOs: {1}", 
                Self.Path.Name, 
                String.Join(", ", toDos.Select(t => t.Id))
            );
        }

        // ein ToDo wird nach vorne verschoben
        private void ToDoMovedUp(ToDoMovedUp toDoMovedUp)
        {
            Context.System.Log.Debug("Actor {0}: process {1}", Self.Path.Name, toDoMovedUp);

            var position = toDos.FindIndex(x => x.Id == toDoMovedUp.Id);
            if (position != -1)
            {
                var todo = toDos[position];
                toDos.RemoveAt(position);
                toDos.Insert(0, todo);
            }
        }
    }
}
