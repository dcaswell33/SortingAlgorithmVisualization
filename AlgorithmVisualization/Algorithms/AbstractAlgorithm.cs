using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgorithmVisualization.Algorithms
{
    public abstract class AbstractAlgorithm
    {
        protected abstract long InternalRun(IntegerArrayWithEvents data);
        public bool AllowRun { get; set; }

        public async Task<long> Run(IntegerArrayWithEvents data)
        {
            AllowRun = true;
            long result = 0;
            await Task.Run(() =>
            {
                result = InternalRun(data);
                data.OnAlgorithmFinished();
            });
            OnAlgorithmComplete();
            return result;
        }

        public long RunSynchronous(IntegerArrayWithEvents data)
        {
            AllowRun = true;
            long numberOps = InternalRun(data);
            data.OnAlgorithmFinished();
            return numberOps;
        }

        protected abstract string GetCodeString { get; }

        private string[] stringSeparators = new string[] { "\r\n" };
        public string[] CodeString()
        {
            string[] lines = GetCodeString.Split(stringSeparators, StringSplitOptions.None);

            return lines;//.ToList();// List<string>(lines);
        }

        public List<string> CodeStringAsList => new List<string>(CodeString());


        public delegate void AlgorithmCompleteHandler(object sender, EventArgs e);
        public event AlgorithmCompleteHandler AlgorithmComplete;
        protected void OnAlgorithmComplete()
        {
            AlgorithmComplete?.Invoke(this, new EventArgs());
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
        public abstract string[] QuestionAnswers { get; }

        public virtual bool GenerateLostSort { get; private set; } = true;
    }
}
