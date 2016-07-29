using System;

namespace ToDoList.Actors
{
    public class Handler
    {
        public Type Type { get; private set; }
        public Action<object> Action { get; private set; }

        public Handler(Type type, Action<object> action)
        {
            Type = type;
            Action = action;
        }
    }
}
