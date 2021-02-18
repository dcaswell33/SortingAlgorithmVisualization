namespace AlgorithmVisualization.Algorithms
{
    public class MergeSort : AbstractAlgorithm
    {
        const string MergeSortCode =
@"void Merge(int numbers[], int i, int j, int k) {
   int mergedSize = k - i + 1;                // Size of merged partition
   int mergePos = 0;                          // Position to insert merged number
   int leftPos = i;                           // Position of elements in left partition
   int rightPos = j + 1;                      // Position of elements in right partition
   int mergedNumbers[] = new int[mergedSize]; // Dynamically allocates temporary array
                                              // for merged numbers
   
   // Add smallest element from left or right partition to merged numbers
   while (leftPos <= j && rightPos <= k) {
      if (numbers[leftPos] <= numbers[rightPos]) {
         mergedNumbers[mergePos] = numbers[leftPos];
         leftPos++;
      }
      else {
         mergedNumbers[mergePos] = numbers[rightPos];
         rightPos++;         
      }
      mergePos++;
   }
   
   // If left partition is not empty, add remaining elements to merged numbers
   while (leftPos <= j) {
      mergedNumbers[mergePos] = numbers[leftPos];
      leftPos++;
      mergePos++;
   }
   
   // If right partition is not empty, add remaining elements to merged numbers
   while (rightPos <= k) {
      mergedNumbers[mergePos] = numbers[rightPos];
      rightPos++;
      mergePos++;
   }
   
   // Copy merge number back to numbers
   for (mergePos = 0; mergePos < mergedSize; ++mergePos) {
      numbers[i + mergePos] = mergedNumbers[mergePos];
   }
}

void MergeSort(int numbers[], int i, int k) {
   int j = 0;
   
   if (i < k) {
      j = (i + k) / 2;  // Find the midpoint in the partition
      
      // Recursively sort left and right partitions
      MergeSort(numbers, i, j);
      MergeSort(numbers, j + 1, k);
      
      // Merge left and right partition in sorted order
      Merge(numbers, i, j, k);
   }
}";

        protected override string GetCodeString => MergeSortCode;
        private long _operationCount = 0;
        private int N;
        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            _operationCount = 0;
            N = data.Count;
            mergeSort(data, 0, data.Count - 1);
            return _operationCount;
        }

        private void mergeSort(IntegerArrayWithEvents array, int lBound, int rBound)
        {
            if (lBound < rBound)
            {
                if (!AllowRun) return;
                int mid = (lBound + rBound) / 2;
                mergeSort(array, lBound, mid);
                mergeSort(array, mid + 1, rBound);
                merge(array, lBound, mid, rBound);
            }
        }

        private void merge(IntegerArrayWithEvents array, int lBound, int mid, int rBound)
        {
            int j, k, i;
            int[] tempArray = new int[N];
            // Copy the first sublist into the tempArray
            for (j = lBound; j <= mid; j++)
            {
                if (!AllowRun) return;
                tempArray[j] = array[j];
                _operationCount++;
            }
            // Copy the second sublist into the tempArray
            for (j = mid + 1, k = rBound; j <= rBound; j++, k--)
            {
                if (!AllowRun) return;
                tempArray[k] = array[j];
                _operationCount++;
            }

            // Merge the two sublists
            j = lBound;
            k = rBound;
            i = lBound;
            while (j <= k)
            {
                if (!AllowRun) return;
                _operationCount++;
                if (tempArray[j] < tempArray[k])
                {
                    array[i] = tempArray[j];
                    j++;
                }
                else
                {
                    array[i] = tempArray[k];
                    k--;
                }
                i++;
            }
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
            "o(nlogn)",
            "o(nlogn)",
            "o(nlogn)",
            "works on sections of the data",
            //"complete passes",
            //"separate values into smaller and larger",
            "sorts in groups and combines",
            //"final position immediately",
            //"moved many times in succession",
            //"dramatically different depending",
            "merge sort"
            };
    }
}
