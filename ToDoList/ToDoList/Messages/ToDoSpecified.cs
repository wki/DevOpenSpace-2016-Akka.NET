using System;

namespace ToDoList.Messages
{
    /// <summary>
    /// Event der anzeigt, dass ein ToDo spezifiziert wurde.
    /// </summary>
    public class ToDoSpecified : SpecifyToDo
    {
        
        public ToDoSpecified(string id, string description)
            : base(id, description) {}
        
//        public ToDoSpecified(SpecifyToDo specifyToDo)
//            : this(specifyToDo.Id, specifyToDo.Description) {}

        public override string ToString()
        {
            return string.Format("[ToDoSpecified]");
        }
    }
}
