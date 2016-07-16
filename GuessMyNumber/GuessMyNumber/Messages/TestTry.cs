namespace GuessMyNumber.Messages
{
    /// <summary>
    /// Message send to Selector to test a given try
    /// </summary>
    public class TestTry
    {
        public int Number { get; private set; }

        public TestTry(int number)
        {
            Number = number;
        }
    }
}
