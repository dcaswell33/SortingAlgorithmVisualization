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
                tempArray[j] = array[j];
                _operationCount++;
            }
            // Copy the second sublist into the tempArray
            for (j = mid + 1, k = rBound; j <= rBound; j++, k--)
            {
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
                if (tempArray[j] < tempArray[k])
                {
                    array[i] = tempArray[j];
                    _operationCount++;
                    j++;
                }
                else
                {
                    array[i] = tempArray[k];
                    k--;
                    _operationCount ++;
                }
                i++;
            }
        }
    }
}
