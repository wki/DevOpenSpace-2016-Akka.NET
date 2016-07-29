using System;
using Akka.Actor;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoList.Messages;
using ToDoList.Actors;

namespace ToDoList
{
    public class ToDoService : IToDoService
    {
        // wir haben nur einen Service der mit dem Actor Modell spricht
        // daher genügt dieser einfache Ansatz.
        // hätten wir mehr, müssten wir das ActorSystem übergeben.

        // unser Actor System
        private readonly ActorSystem actorSystem;

        // das "Write" Model
        private readonly IActorRef todoDispatcher;

        // das "Read" Model
        private readonly IActorRef todos;

        /// <summary>
        /// Konstruktor. Eventuell sollten wir das ActorSystem hier übergeben
        /// </summary>
        public ToDoService()
        {
            actorSystem = ActorSystem.Create("ToDo");

            todoDispatcher = actorSystem.ActorOf(Props.Create<ToDoDispatcher>(), "ToDoDispatcher");
            todos = actorSystem.ActorOf(Props.Create<ToDos>(), "ToDos");
        }

        // ---------------- "Write Model" Use Cases

        /// <summary>
        /// Specifies the todo by creating or changing a ToDos info
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        public void SpecifyToDo(string id, string description)
        {
            todoDispatcher.Tell(new SpecifyToDo(id, description));
        }

        /// <summary>
        /// Moves a ToDo to the frontmost position
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void MoveUpToDo(string id)
        {
            todoDispatcher.Tell(new MoveUpToDo(id));
        }

        // ---------------- "Read Model" Use Cases

        /// <summary>
        /// Lists all to dos.
        /// </summary>
        /// <returns>The to dos.</returns>
        public Task<IEnumerable<ToDoDocument>> ListToDos()
        {
            return todos.Ask<IEnumerable<ToDoDocument>>(new ListToDos());
        }
    }
}
