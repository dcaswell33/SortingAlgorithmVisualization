using AlgorithmVisualization.Algorithms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AlgorithmVisualization
{
    public class AnalysisRun
    {
        public int NumberItems { get; set; }
        public long ExecutionTime { get; set; }
        public long NumberOperations { get; set; }
        private IntegerArrayWithEvents.InitialOrdering Order { get; set; }
        private IntegerArrayWithEvents _data;
        private AbstractAlgorithm _algorithm;

        public AnalysisRun(int numberItems, IntegerArrayWithEvents.InitialOrdering ordering, AbstractAlgorithm algorithm)
        {
            _algorithm = algorithm;
            NumberItems = numberItems;
            Order = ordering;
            _data = new IntegerArrayWithEvents(numberItems, numberItems);
            _data.MixNumbersWithoutEvents(Order);
        }

        public bool IsSorted() => _data.TestIfIsSortedAscending();

        public async Task RunAlgorithmAsyncrhonousForTimer()
        {
            //System.Diagnostics.Debug.WriteLine("Running with " + _data.Count + ".  Sorted? " + _data.TestIfIsSortedAscending());
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            NumberOperations = await _algorithm.Run(_data);
            sw.Stop();
            //System.Diagnostics.Debug.WriteLine("Completed run with " + _data.Count + " in " + ExecutionTime + ".  Sorted? " + _data.TestIfIsSortedAscending());            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);

            ExecutionTime = sw.ElapsedMilliseconds;
        }

        public void RunAlgorithmForTimerSynchronous()
        {
            System.Diagnostics.Debug.WriteLine("Running with " + _data.Count + ".  Sorted? " + _data.TestIfIsSortedAscending());
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            NumberOperations = _algorithm.RunSynchronous(_data);
            sw.Stop();
            ExecutionTime = sw.ElapsedMilliseconds;

            System.Diagnostics.Debug.WriteLine("Completed run with " + _data.Count + " in " + ExecutionTime + ".  Sorted? " + _data.TestIfIsSortedAscending());            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);

            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);
        }
    }
}
