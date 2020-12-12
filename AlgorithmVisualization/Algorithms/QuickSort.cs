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
            _operationCount++;
            int lastSmall = lBound;
            for (int i = lBound + 1; i <= rBound; i++)
            {
                if (array[i] < array.TempInteger)
                {
                    lastSmall++;
                    array.Swap(lastSmall, i);
                    _operationCount+=4;
                }
            }
            array.Swap(lBound, lastSmall);
            _operationCount += 3;
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

    }
}
