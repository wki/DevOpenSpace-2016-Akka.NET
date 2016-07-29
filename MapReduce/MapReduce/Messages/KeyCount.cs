namespace MapReduce.Messages
{
    /// <summary>
    /// save a tuple of a key and its count
    /// </summary>
    public class KeyCount
    {
        public string Key { get; set; }
        public int Count { get; set; }

        public KeyCount(string key, int count = 1)
        {
            Key = key;
            Count = count;
        }
    }
}
