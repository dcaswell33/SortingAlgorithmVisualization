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

    }
}
