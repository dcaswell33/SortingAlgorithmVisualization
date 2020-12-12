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

                    // Swap numbers[j] and numbers[j - 1]
                    data.Swap(j, j - 1);                    
                    j--;
                    count += 3;
                }
            }
            return count;
        }

        protected override string GetCodeString => INSERTIONSORTCODE;
    }
}
