using System;
using System.Collections.Generic;

namespace AlgorithmVisualization.Algorithms
{
    public class RadixSort : AbstractAlgorithm
    {
        private const string RADIXSORTCODE =
@"// Returns the maximum length, in number of digits, out of all elements in the array
int RadixGetMaxLength(int array[], int arraySize) {
   int maxDigits = 0;
   int digitCount, i;
   for (i = 0; i < arraySize; i++) {
      digitCount = RadixGetLength(array[i]);
      if (digitCount > maxDigits)
         maxDigits = digitCount;
   }
   return maxDigits;
}

// Returns the length, in number of digits, of value
int RadixGetLength(int value) {
   if (value == 0)
      return 1;

   int digits = 0;
   while (value != 0) {
      digits = digits + 1;
      value = value / 10;
   }
   return digits;
}

void RadixSort(int array[], int arraySize) {
   buckets = create array of 10 buckets

   // Find the max length, in number of digits
   maxDigits = RadixGetMaxLength(array, arraySize);
        
   // Start with the least significant digit
   pow10 = 1;
   for (digitIndex = 0; digitIndex < maxDigits; digitIndex++) {
      for (i = 0; i < arraySize; i++) {
         bucketIndex = abs(array[i] / pow10) % 10;
         Append array[i] to buckets[bucketIndex];
      }
      arrayIndex = 0;
      for (i = 0; i < 10; i++) {
         for (j = 0; j < buckets[i].size(); j++)
            array[arrayIndex++] = buckets[i][j];
      }
      pow10 = 10 * pow10;
      Clear all buckets
   }
}";

        protected override string GetCodeString => RADIXSORTCODE;

        public override string[] QuestionAnswers => new string[] {
            "o(n)",
            "o(n)",
            "o(n)",
            //"works on sections of the data",
            "complete passes",
            "separate values into smaller and larger",
            "sorts in groups and combines",
            //"final position immediately",
            //"moved many time in succession",
            //"dramatically different depending",
            "radix sort"
            };

        public override bool GenerateLostSort => false;

        private int RadixGetMaxLength(IntegerArrayWithEvents data)
        {
            int maxValue = data.MaxValue;
            return Convert.ToInt32(Math.Floor(Math.Log10(Convert.ToDouble(maxValue)) + 1));
        }

        public static int GetDigitInvertedIndex(int value, int digitIndex)
        {

            int place = value / (int) Math.Pow(10, digitIndex) % 10;
            return place;
            //return ((value % (place * 10)) - (value % place)) / place;
        }

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            long count = 0;
            List<List<int>> buckets = new List<List<int>>();

            //buckets = create array of 10 buckets
            for (int i = 0; i < 10; i++)
            {
                buckets.Add(new List<int>());
            }

            // Find the max length, in number of digits
            int maxDigits = RadixGetMaxLength(data);

            // Start with the least significant digit
            
            for (int digitIndex = 0; digitIndex < maxDigits; digitIndex++)
            {
                // Put all of the items into the bucket related to the digit
                for (int i = 0; i < data.Count; i++)
                {
                    count++;
                    int bucketIndex = GetDigitInvertedIndex(data[i], digitIndex);
                    buckets[bucketIndex].Add(data[i]);
                }

                int arrayIndex = 0;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < buckets[i].Count; j++)
                        data[arrayIndex++] = buckets[i][j];
                }

                //Reset the buckets
                for (int i = 0; i < 10; i++)
                {
                    buckets[i].Clear();
                }

            }
            return count;
        }
    }
}
