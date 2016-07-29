namespace ToDoList.Messages
{
    /// <summary>
    /// Command message for specifying a ToDo
    /// </summary>
    public class SpecifyToDo : ToDoDocument
    {
        public SpecifyToDo(string id, string description) : base(id, description) {}

        public override string ToString()
        {
            return string.Format("[SpecifyToDo]");
        }
    }
}
