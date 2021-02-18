namespace AlgorithmVisualization.Algorithms
{
    public class BubbleSort : AbstractAlgorithm
    {
        private const string BUBBLESORTCODE =
@"void BubbleSort(int numbers[], int numbersSize) {
   int i, j, temp;

   for (i = 0; i < numbersSize - 1; i++) {
      for (j = 0; j < numbersSize - i - 1; j++) {
         if (numbers[j] > numbers[j+1]) {
            temp = numbers[j];
            numbers[j] = numbers[j + 1];
            numbers[j + 1] = temp;
         }
      }
   }
}";

        protected override string GetCodeString => BUBBLESORTCODE;

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            bool isSorted = false;
            long count = 0;

            while (!isSorted)
            {
                isSorted = true;
                for (int i = 0; i < data.Count - 1; i++)
                {
                    if (!AllowRun) return count;

                    count++;
                    if (data[i] > data[i + 1])
                    {
                        data.Swap(i, i + 1);
                        isSorted = false;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Answer the questions (in order) - must have unique substring to compare:
        /// 1) worst case random order
        /// 2) worst case ascending order
        /// 3) worst case descending order
        /// 4) attributes from listSorting Algorithms
        ///     Works on sections of the data, then moves to the next section
        ///     Makes complete passes of the data to incorporate each element into a sorted list
        ///     Appears to use separate values into smaller and larger groups without explicit sort
        ///     Sorts in groups and combines those groups
        ///     Moves values into final position immediately (not to some other place first)
        ///     (Some) data values are moved many times in succession before arriving at sorted position
        ///     Run-time is dramatically different depending on initial order of values
        /// 5) what sorting algorithm is it
        /// </summary>
        public override string[] QuestionAnswers => new string[] {
            "o(n^2)",
            "o(n)",
            "o(n^2)",
            //"works on sections of the data",
            "complete passes",
            //"separate values into smaller and larger",
            //"sorts in groups and combines",
            //"final position immediately",
            "moved many times in succession",
            "dramatically different depending",
            "bubble sort"
            };
    }
}
