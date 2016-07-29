using System;
using Akka.Actor;
using ToDoList.Messages;

namespace ToDoList.Actors
{
    /// <summary>
    /// Das "Write Model". Verteilt jeden schreibenden Zugriff auf einen ToDo Aktor
    /// </summary>
    public class ToDoDispatcher : ReceiveActor
    {
        public ToDoDispatcher()
        {
            Receive<SpecifyToDo>(s => SpecifyToDo(s));
            Receive<MoveUpToDo>(m => MoveUpToDo(m));
        }

        private void SpecifyToDo(SpecifyToDo specifyToDo)
        {
            Context.System.Log.Debug("Actor {0}: process {1}", Self.Path.Name, specifyToDo);

            var id = specifyToDo.Id;
            EnsureToDoExists(id); // legt fehlenden Aktor an

            Context.Child(id).Forward(specifyToDo);
        }

        private void MoveUpToDo(MoveUpToDo moveUpToDo)
        {
            Context.System.Log.Debug("Actor {0}: process {1}", Self.Path.Name, moveUpToDo);

            var id = moveUpToDo.Id;
            EnsureToDoExists(id); // legt fehlenden Aktor an

            Context.Child(id).Forward(moveUpToDo);
        }

        private void EnsureToDoExists(string id)
        {
            if (Context.Child(id) != Nobody.Instance)
                return;

            Context.System.Log.Debug("Actor {0}: creating ToDo Actor {1}", Self.Path.Name, id);

            Context.ActorOf(Props.Create<ToDo>(id), id);
        }
    }
}
