namespace AlgorithmVisualization.Algorithms
{
    public class BubbleSort : AbstractAlgorithm
    {
        private const string BUBBLESORTCODE =
@"void BubbleSort(int numbers[], int numbersSize) {
   int i, j, temp;

   for (i = 0; i < numbersSize - 1; i++) {
      for (j = 0; j < numbersSize - i - 1; j++) {
         if (numbers[j] > numbers[j+1]) {
            temp = numbers[j];
            numbers[j] = numbers[j + 1];
            numbers[j + 1] = temp;
         }
      }
   }
}";

        protected override string GetCodeString => BUBBLESORTCODE;

        protected override long InternalRun(IntegerArrayWithEvents data)
        {
            bool isSorted = false;
            long count = 0;

            while (!isSorted)
            {
                isSorted = true;
                for (int i = 0; i < data.Count - 1; i++)
                {
                    if (!AllowRun) return count;

                    if (data[i] > data[i+1])
                    {
                        data.Swap(i, i + 1);
                        isSorted = false;
                        count += 3;
                    }
                }
            }
            return count;
        }
    }
}
