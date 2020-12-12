namespace AlgorithmVisualization
{
    public class ExecutionTiming
    {
        public int NumberDataItems { get; private set; }
        public long TimeInMilliseconds { get; private set; }

        public ExecutionTiming(int numDataItems, long timeInMilliseconds)
        {
            NumberDataItems = numDataItems;
            TimeInMilliseconds = timeInMilliseconds;
        }
    }


}
