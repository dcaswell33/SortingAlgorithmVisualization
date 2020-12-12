using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AlgorithmVisualization
{
    public class IntegerArrayWithEvents
    {
        public enum InitialOrdering
        {
            Random,
            Ascending,
            Descending
        };

        private int _maxValue;
        public int MaxValue { get => _maxValue; set => _maxValue = value; }

        private int _tempInteger;
        public int TempInteger { get => _tempInteger;
            set
            {
                _tempInteger = value;
                OnTempVariableChanged(new ElementChangedEventArgs(-1, _tempInteger));
            }
        }

        private int _amount;
        public int Count { get => _amount; set => _amount = value; }

        public int DelayMs { get; set; } = 0;

        private readonly Random _rand = new Random();

        private List<int> _internalList;

        public IntegerArrayWithEvents(int amount, int maxValue)
        {
            ReInitializeData(amount, maxValue);
        }

        public void ReInitializeData(int amount, int maxValue)
        {
            _maxValue = maxValue;
            _internalList = new List<int>(amount);
            _amount = amount;
            GenerateOrdinalNumbers();
        }

        private void GenerateNewRandomNumbers()
        {
            for (int i = 0; i < _amount; i++)
            {
                _internalList.Add(_rand.Next(_maxValue));
            }
        }

        private void GenerateOrdinalNumbers()
        {
            for (int i = 0; i < _amount; i++)
            {
                _internalList.Add(i + 1);
            }
            _maxValue = _amount;
        }

        public bool TestIfIsSortedAscending()
        {
            int lastItem = -1;
            foreach(int item in _internalList)
            {
                if (item < lastItem) return false;
                lastItem = item;
            }
            return true;
        }

        public void MixNumbersWithEvents(InitialOrdering ordering)
        {
            switch (ordering)
            {
                case InitialOrdering.Ascending:
                    _internalList.Sort((a, b) => a.CompareTo((b)));
                    break;
                case InitialOrdering.Descending:
                    _internalList.Sort((a, b) => -1 * a.CompareTo(b));
                    break;
                case InitialOrdering.Random:
                    for (int i = 0; i < _internalList.Count; i++)
                    {
                        int j = _rand.Next(_internalList.Count);
                        Swap(i, j);
                    }
                    break;
            }
        }

        public void MixNumbersWithoutEvents(InitialOrdering ordering)
        {
            switch (ordering)
            {
                case InitialOrdering.Ascending:
                    _internalList.Sort((a,b) => a.CompareTo((b)));
                    break;
                case InitialOrdering.Descending:
                    _internalList.Sort((a,b) => -1*a.CompareTo(b));
                    break;
                case InitialOrdering.Random:
                    for (int i = 0; i < _internalList.Count; i++)
                    {
                        int j = _rand.Next(_internalList.Count);
                        int temp = _internalList[i];
                        _internalList[i] = _internalList[j];
                        _internalList[j] = temp;
                    }
                    break;
            }
        }

        public void Swap(int indexI, int indexJ)
        {
            int temp = _internalList[indexI];
            _internalList[indexI] = _internalList[indexJ];
            _internalList[indexJ] = temp;
            OnElementSwapped(new ElementsSwappedEventArgs(indexI, indexJ));
            if (DelayMs > 0) Thread.Sleep(DelayMs);
        }

        public int this[int index]
        {
            get {
                OnElementExamined( new ElementExaminedEventArgs(index));
                if (DelayMs > 0) Thread.Sleep(DelayMs);
                return _internalList[index];
            }
            set
            {
                _internalList[index] = value;
                OnElementChanged(new ElementChangedEventArgs(index));
                if (DelayMs > 0) Thread.Sleep(DelayMs);
            }

        }

        public int GetValueWithoutEvents(int index)
        {
            return _internalList[index];
        }


        #region Events & Delegates

        #region Swap Event
        public delegate void SwapEventHandler(ElementsSwappedEventArgs e);
        public event SwapEventHandler ElementsSwapped;

        protected virtual void OnElementSwapped(ElementsSwappedEventArgs e)
        {
            ElementsSwapped?.Invoke(e);
        }

        public bool IsElementSwappedEventHandlerRegistered(Delegate prospectiveHandler)
        {
            if (ElementsSwapped != null)
            {
                return ((IList<Delegate>)ElementsSwapped.GetInvocationList()).Contains(prospectiveHandler);
            }
            return false;
        }

        #endregion

        #region Examine Event
        public delegate void ElementExaminedEventHandler(ElementExaminedEventArgs e);
        public event ElementExaminedEventHandler ElementExamined;

        protected virtual void OnElementExamined(ElementExaminedEventArgs e)
        {
            ElementExamined?.Invoke(e);
        }

        public bool IsElementExaminedEventHandlerRegistered(Delegate prospectiveHandler)
        {
            if (ElementExamined != null)
            {
                return ((IList<Delegate>)ElementExamined.GetInvocationList()).Contains(prospectiveHandler);
            }
            return false;
        }

        #endregion

        #region Value/Element Changed
        public delegate void ElementChangedEventHandler(ElementChangedEventArgs e);
        public event ElementChangedEventHandler ElementChanged;

        protected void OnElementChanged(ElementChangedEventArgs e)
        {
            ElementChanged?.Invoke(e);
        }
        #endregion

        #region TempVariable Changed
        public delegate void TempVariableChangedEventHandler(ElementChangedEventArgs e);
        public event TempVariableChangedEventHandler TempVariableChanged;

        protected void OnTempVariableChanged(ElementChangedEventArgs e)
        {
            TempVariableChanged?.Invoke(e);
        }
        #endregion

        #region Algorithm Complete
        public delegate void AlgorithmFinishedEventHandler(EventArgs e);
        public event AlgorithmFinishedEventHandler AlgorithmFinished;
    
        public void OnAlgorithmFinished()
        {
            AlgorithmFinished?.Invoke(new EventArgs());
        }
        #endregion

        #endregion


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_internalList.Count > 0) sb.Append(_internalList[0]);
            for (int i =1; i < _internalList.Count; i++)
            {
                sb.Append(", ").Append(_internalList[i]);
            }
            return sb.ToString();
        }
    }

    public class ElementsSwappedEventArgs : EventArgs
    {
        public int FirstIndex { get; set; }
        public int SecondIndex { get; set; }

        public ElementsSwappedEventArgs(int i, int j)
        {
            FirstIndex = i;
            SecondIndex = j;
        }
    }

    public class ElementExaminedEventArgs : EventArgs
    {
        public int Index { get; set; }

        public ElementExaminedEventArgs(int i)
        {
            Index = i;
        }
    }

    public class ElementChangedEventArgs : EventArgs
    {
        public int Index { get; set; }
        public int Value { get; set; } = -1;

        public ElementChangedEventArgs(int i)
        {
            Index = i;
        }

        public ElementChangedEventArgs(int i, int val)
        {
            Index = i;
            Value = val;
        }
    }
}
