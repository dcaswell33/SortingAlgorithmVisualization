using System;
using System.Collections.Generic;

namespace AlgorithmVisualization.Algorithms
{
    public class ShellSort : AbstractAlgorithm
    {
        private const string Shellsortcode =
@"void InsertionSortInterleaved(int numbers[], int numbersSize, int startIndex, int gap) {
   int i = 0;
   int j = 0;
   int temp = 0;  // Temporary variable for swap

   for (i = startIndex + gap; i < numbersSize; i = i + gap) {
      j = i;
      while (j - gap >= startIndex && numbers[j] < numbers[j - gap]) {
         temp = numbers[j];
         numbers[j] = numbers[j - gap];
         numbers[j - gap] = temp;
         j = j - gap;
      }
   }
}

void ShellSort(int numbers[], int numbersSize, int gapValues[], int numberGaps) {
    int i = 0;
    int gapIndex = 0;
    int gapValue;
    
    for (gapIndex=0; gapIndex < numberGaps; gapIndex++) {
      gapValue = gapValues[gapIndex];
      for (int i = 0; i < gapValue; i++) {
         InsertionSortInterleaved(numbers, numbersSize, i, gapValue)
      }
   }
}";
        protected override string GetCodeString => Shellsortcode;

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            long count = 0;
            List<int> k = new List<int>();
            int i = 1;
            k.Add(1);
            count++;
            while ( (Math.Pow(2,i)+1) < data.Count)
            {
                k.Add((int) Math.Pow(2,i) + 1);
                i++;
                count+=2;
            }
            k.Reverse();
            count += data.Count;

            for (int x = 0; x < k.Count; x++)
            { // sort based on each element of k[]
                for (i = k[x]; i <= data.Count - 1; i++)
                {

                    if (!AllowRun) return count;
                    int j = i - k[x];
                    int val = data[i];
                    count += 2;
                    // insertion sort on i'th subset
                    while (j >= 0 && val < data[j])
                    {
                        data[j + k[x]] = data[j]; // copy over until we find insertion point
                        j = j - k[x];
                        count += 2;
                    }
                    data[j + k[x]] = val; // put value in its correct position
                    count++;
                }
            }
            return count;
        }
    }
}
