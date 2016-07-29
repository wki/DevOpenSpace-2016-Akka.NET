namespace ToDoList.Messages
{
    /// <summary>
    /// Command message forcing a restore operation
    /// </summary>
    public class RestoreFromJournal 
    {
        public string Glob { get; private set; }

//        static RestoreFromJournal _instance;
//        public static RestoreFromJournal Instance 
//        { 
//            get 
//            { 
//                return _instance ?? (_instance = new RestoreFromJournal()); 
//            }
//        }

        public RestoreFromJournal(string glob)
        {
            Glob = glob;
        }

        public override string ToString()
        {
            return string.Format("[RestoreFromJournal: Glob={0}]", Glob);
        }
    }
}
