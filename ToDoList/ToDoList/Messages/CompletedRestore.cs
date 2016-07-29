namespace ToDoList.Messages
{
    /// <summary>
    /// Event message sent as soon as a restore is complete
    /// </summary>
    public class CompletedRestore
    {
        static CompletedRestore _instance;

        public static CompletedRestore Instance 
        { 
            get 
            { 
                return _instance ?? (_instance = new CompletedRestore()); 
            }
        }

        public override string ToString()
        {
            return string.Format("[CompletedRestore: ]");
        }
    }
}
