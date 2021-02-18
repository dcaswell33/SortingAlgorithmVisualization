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
            while ( (Math.Pow(2,i)+1) < data.Count)
            {
                k.Add((int) Math.Pow(2,i) + 1);
                i++;
            }
            k.Reverse();

            for (int x = 0; x < k.Count; x++)
            { // sort based on each element of k[]
                for (i = k[x]; i <= data.Count - 1; i++)
                {

                    if (!AllowRun) return count;
                    int j = i - k[x];
                    int val = data[i];
                    // insertion sort on i'th subset
                    while (j >= 0 && val < data[j])
                    {
                        data[j + k[x]] = data[j]; // copy over until we find insertion point
                        j = j - k[x];
                        count ++;
                    }
                    count++;
                    data[j + k[x]] = val; // put value in its correct position
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
            "o(n^1.5)",
            "o(n^1.5)",
            "o(n^1.5)",
            "works on sections of the data",
            //"complete passes",
            //"separate values into smaller and larger",
            //"sorts in groups and combines",
            //"final position immediately",
            "moved many times in succession",
            //"dramatically different depending",
            "shell sort"
            };
    }
}
