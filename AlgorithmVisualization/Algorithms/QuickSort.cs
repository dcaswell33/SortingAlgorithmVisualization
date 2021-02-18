namespace AlgorithmVisualization.Algorithms
{
    public class QuickSort : AbstractAlgorithm
    {
        private const string QUICKSORTSTRING =
@"void Partition(int numbers[], int i, int k) {
   int l = 0;
   int h = 0;
   int midpoint = 0;
   int pivot = 0;
   int temp = 0;
   bool done = false;
   
   // Pick middle element as pivot
   midpoint = i + (k - i) / 2;
   pivot = numbers[midpoint];
   
   l = i;
   h = k;
   
   while (!done) {
      
      // Increment l while numbers[l] < pivot
      while (numbers[l] < pivot) {
         l++;
      }
      
      // Decrement h while pivot < numbers[h]
      while (pivot < numbers[h]) {
         h--;
      }
      
      // If there are zero or one elements remaining,
      // all numbers are partitioned. Return h
      if (l >= h) {
         done = true;
      }
      else {
         // Swap numbers[l] and numbers[h],
         // update l and h
         temp = numbers[l];
         numbers[l] = numbers[h];
         numbers[h] = temp;
         
         l++;
         h--;
      }
   }
   
   return h
}

void Quicksort(int numbers[], int i, int k) {
   j = 0;
   
   // Base case: If there are 1 or zero elements to sort,
   // partition is already sorted
   if (i >= k) {
      return;
   }
   
   // Partition the data within the array. Value j returned
   // from partitioning is location of last element in low partition.
   j = Partition(numbers, i, k);
   
   // Recursively sort low partition (i to j) and
   // high partition (j + 1 to k)
   Quicksort(numbers, i, j);
   Quicksort(numbers, j + 1, k);
}";
        protected override string GetCodeString => QUICKSORTSTRING;
        private long _operationCount;
        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            _operationCount = 0;
            quickSort(data, 0, data.Count - 1);
            return _operationCount;
        }

        private int partition(IntegerArrayWithEvents array, int lBound, int rBound)
        {
            array.TempInteger = array[lBound];  //Pivot
            int lastSmall = lBound;
            for (int i = lBound + 1; i <= rBound; i++)
            {
                _operationCount++;
                if (array[i] < array.TempInteger)
                {
                    if (!AllowRun) return lastSmall;
                    lastSmall++;
                    array.Swap(lastSmall, i);
                }
            }
            array.Swap(lBound, lastSmall);
            return lastSmall;  //return the division point
        }

        private void quickSort(IntegerArrayWithEvents array, int lBound, int rBound)
        {
            if (lBound < rBound)
            {
                if (!AllowRun) return;
                int divPt = partition(array, lBound, rBound);
                quickSort(array, lBound, divPt - 1);
                quickSort(array, divPt + 1, rBound);
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
            "o(n^2)",
            "o(n^2)",
            "works on sections of the data",
            //"complete passes",
            "separate values into smaller and larger",
            //"sorts in groups and combines",
            //"final position immediately",
            "moved many times in succession",
            "dramatically different depending",
            "quick sort"
            };
    }
}
