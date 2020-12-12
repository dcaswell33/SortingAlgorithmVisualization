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
                count += 2;
                for (int j = i + 1; j < data.Count; j++)
                {

                    if (!AllowRun) return count;
                    if (data[j] < smallest)
                    {
                        smallest = data[j];
                        smallIndex = j;
                        count += 2;
                    }
                }
                // once found swap it with the value in the ith
                // position
                data.Swap(i, smallIndex);
                count += 3;
            }
            return count;

        }
    }
}
