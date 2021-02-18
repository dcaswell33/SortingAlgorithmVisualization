namespace AlgorithmVisualization.Algorithms
{
    public class SelectionSort : AbstractAlgorithm
    {
        private const string SELECTIONSORTCODE =
 @"void SelectionSort(int numbers[], int numbersSize) {
   int i = 0;
   int j = 0;
   int indexSmallest = 0;
   int temp = 0;  // Temporary variable for swap
   
   for (i = 0; i < numbersSize - 1; i++) {
      
      // Find index of smallest remaining element
      indexSmallest = i;
      for (j = i + 1; j < numbersSize; j++) {
         
         if ( numbers[j] < numbers[indexSmallest] ) {
            indexSmallest = j;
         }
      }
      
      // Swap numbers[i] and numbers[indexSmallest]
      temp = numbers[i];
      numbers[i] = numbers[indexSmallest];
      numbers[indexSmallest] = temp;
   }
}";

        protected override string GetCodeString => SELECTIONSORTCODE;

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            long count = 0;
            int smallest = 0;
            int smallIndex = 0;
            // process the array from left to right
            for (int i = 0; i < data.Count; i++)
            {
                // look for smallest value in the array
                smallest = data[i];
                smallIndex = i;
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (!AllowRun) return count;
                    count++;
                    if (data[j] < smallest)
                    {
                        smallest = data[j];
                        smallIndex = j;
                    }
                }
                // once found swap it with the value in the ith
                // position
                data.Swap(i, smallIndex);
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
            "o(n^2)",
            "o(n^2)",
            //"works on sections of the data",
            "complete passes",
            //"separate values into smaller and larger",
            //"sorts in groups and combines",
            "final position immediately",
            //"moved many times in succession",
            //"dramatically different depending",
            "selection sort"
            };
    }
}
