namespace AlgorithmVisualization.Algorithms
{
    public class InsertionSort : AbstractAlgorithm
    {
        private const string INSERTIONSORTCODE =
 @"void InsertionSort(int numbers[], int numbersSize) {
   int i, j, temp;
   
   for (i = 1; i < numbersSize; ++i) {
      j = i;
      // Insert numbers[i] into sorted part
      // stopping once numbers[i] in correct position
      while (j > 0 && numbers[j] < numbers[j - 1]) {
         
         // Swap numbers[j] and numbers[j - 1]
         temp = numbers[j];
         numbers[j] = numbers[j - 1];
         numbers[j - 1] = temp;
         --j;
      }
   }
}";

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            //int i = 1;
            //while (i < data.Count)
            //{
            //    data.TempInteger = data[i];
            //    int j = i - 1;
            //    while (j >= 0 && data[j] > data.TempInteger)
            //    {
            //        if (!AllowRun) return;
            //        data[j + 1] = data[j];
            //        j--;
            //    }
            //    data[j + 1] = data.TempInteger;
            //    i++;
            //}
            long count = 0;
            int i = 0;
            int j = 0;

            for (i = 1; i < data.Count; i++)
            {
                j = i;
                // Insert numbers[i] into sorted part
                // stopping once numbers[i] in correct position
                while (j > 0 && data[j] < data[j - 1])
                {
                    if (!AllowRun) return count;
                    count++;
                    // Swap numbers[j] and numbers[j - 1]
                    data.Swap(j, j - 1);                    
                    j--;
                }
            }
            return count;
        }

        protected override string GetCodeString => INSERTIONSORTCODE;

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
            "insertion sort"
            };
    }
}
