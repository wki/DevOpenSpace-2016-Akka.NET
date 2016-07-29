namespace ToDoList.Messages
{
    /// <summary>
    /// Command Message forcing a message persist action
    /// </summary>
    public class PersistToJournal
    {
        public string PersistenceId { get; private set; }
        public object Message { get; private set; }

        public PersistToJournal(string persistenceId, object message)
        {
            PersistenceId = persistenceId;
            Message = message;
        }

        public override string ToString()
        {
            return string.Format("[PersistToJournal: PersistenceId={0}, Message={1}]", PersistenceId, Message);
        }
    }
}
