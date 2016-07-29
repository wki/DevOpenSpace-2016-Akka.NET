using System;

namespace ToDoList.Messages
{
    public class ToDoMovedUp : MoveUpToDo
    {
        public ToDoMovedUp(string id) : base(id) {}
    }
}
