namespace ToDoList.Messages
{
    /// <summary>
    /// Command message to retrieve the read model content
    /// </summary>
    public class ListToDos 
    {
        public override string ToString()
        {
            return string.Format("[ListToDos]");
        }
    }
}
