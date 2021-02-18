using AlgorithmVisualization.Algorithms;
using System.Threading.Tasks;

namespace AlgorithmVisualization
{
    public class VisualizationRun
    {
        public int NumberItems { get; set; }
        private IntegerArrayWithEvents.InitialOrdering Order { get; set; }
        private IntegerArrayWithEvents _data;
        private AbstractAlgorithm _algorithm;

        public VisualizationRun(int numberItems, IntegerArrayWithEvents.InitialOrdering ordering, AbstractAlgorithm algorithm)
        {
            _algorithm = algorithm;
            NumberItems = numberItems;
            Order = ordering;
            _data = new IntegerArrayWithEvents(numberItems, numberItems);
            _data.MixNumbersWithoutEvents(Order);
        }

        public bool IsSorted() => _data.TestIfIsSortedAscending();

        public async Task Run()
        {
            await _algorithm.Run(_data);
        }

    }
}
