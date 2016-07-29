namespace ToDoList.Messages
{
    /// <summary>
    /// Document message containing a ToDo, explicitly named to avoid conflict with ToDo Actor
    /// </summary>
    public class ToDoDocument
    {
        public string Id { get; private set; }
        public string Description { get; private set; }

        public ToDoDocument (string id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString()
        {
            return string.Format("[ToDoDocument: Id={0}, Description={1}]", Id, Description);
        }
    }
}
