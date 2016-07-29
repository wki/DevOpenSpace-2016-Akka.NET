namespace ToDoList.Messages
{
    /// <summary>
    /// Command message forcing a ToDo to be moved to the front of the list
    /// </summary>
    public class MoveUpToDo
    {
        public string Id { get; private set; }

        public MoveUpToDo(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("[MoveUpToDo: Id={0}]", Id);
        }
    }
}
