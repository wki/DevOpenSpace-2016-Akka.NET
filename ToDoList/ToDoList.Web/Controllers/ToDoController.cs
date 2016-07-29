using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoList.Messages;

namespace ToDoList.Web
{
    /// <summary>
    /// Zugriff auf das "Read Model" und "Write Model"
    /// </summary>
    [RoutePrefix("api/todo")]
    public class ToDoController : ApiController
    {
        private readonly IToDoService todoService;

        public class ToDoData
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }

        public ToDoController(IToDoService todoService)
        {
            this.todoService = todoService;
        }

        /// <summary>
        /// Lesen aller ToDo Einträge
        /// </summary>
        /// <returns>The todos.</returns>
        [HttpGet, Route("")]
        public Task<IEnumerable<ToDoDocument>> ListTodos()
        {
            return todoService.ListToDos();
        }

        /// <summary>
        /// Anlegen oder verändern von ToDo Einträgen.
        /// </summary>
        /// <param name="toDo">ToDo</param>
        /// <description>>
        /// die Verwendung der Methode "POST" für zwei verschiedene Zwecke
        /// ist zwar nicht RESTful, der Einfachkeit halber sei das mal egal...
        /// </description>
        [HttpPost, Route("")]
        public void SpecifyToDo([FromBody] ToDoData toDo)
        {
            todoService.SpecifyToDo(toDo.Id, toDo.Description);
        }

        /// <summary>
        /// Nach oben schieben eines ToDo Eintrages
        /// </summary>
        /// <param name="id">Identifier.</param>
        [HttpPost, Route("{id}/moveup")]
        public void MoveUpToDo([FromUri] string id)
        {
            todoService.MoveUpToDo(id);
        }
    }
}
