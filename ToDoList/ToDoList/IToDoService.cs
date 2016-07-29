using System;
using ToDoList.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList
{
    public interface IToDoService
    {
        void SpecifyToDo(string id, string description);
        void MoveUpToDo(string id);
        Task<IEnumerable<ToDoDocument>> ListToDos();
    }
}
