using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AlgorithmVisualization.Algorithms;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for SortingAlgorithmAnalysis.xaml
    /// </summary>
    public partial class SortingAlgorithmAnalysis : Window
    {
        public SortingAlgorithmAnalysis()
        {
            InitializeComponent();
            SetupObjectivesComboBox();
        }

        #region Algorithm ComboBox GUI
        private Algorithms.AbstractAlgorithm GetObjectiveFunction()
        {
            return (Algorithms.AbstractAlgorithm)Activator.CreateInstance((Type)((ComboBoxItem)ComboBoxAlgorithm.SelectedItem).Tag);
        }

        private void SetupObjectivesComboBox()
        {
            var listOfBs = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(Algorithms.AbstractAlgorithm).IsAssignableFrom(assemblyType)
                select assemblyType).ToArray();

            foreach (var subclass in listOfBs)
            {
                if (!subclass.IsAbstract)
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Content = subclass.Name,
                        Tag = subclass
                    };
                    ComboBoxAlgorithm.Items.Add(newItem);
                }
            }
            ComboBoxAlgorithm.SelectedIndex = 0;
        }
        #endregion

        private int MaximumValue => Convert.ToInt32(ComboBoxDataSize.SelectedValue.ToString());
        private int NumberTestPoints => Convert.ToInt32(NumberRuns.SelectedValue.ToString());

        private IntegerArrayWithEvents.InitialOrdering InitialOrderSelected
        {
            get
            {
                switch (ComboBoxInitialSortingPolicy.SelectedIndex)
                {
                    case 0: return IntegerArrayWithEvents.InitialOrdering.Random;
                    case 1: return IntegerArrayWithEvents.InitialOrdering.Ascending;
                    default: return IntegerArrayWithEvents.InitialOrdering.Descending;
                }
            }
        }

        private void UpdateTable()
        {
            DataGridDisplay.ItemsSource = runs;
        }

        private List<AnalysisRun> runs;

        private void SetActive(bool value)
        {
            ComboBoxInitialSortingPolicy.IsEnabled = value;
            ComboBoxDataSize.IsEnabled = value;
            ComboBoxInitialSortingPolicy.IsEnabled = value;
            ComboBoxAlgorithm.IsEnabled = value;
            NumberRuns.IsEnabled = value;
            RunButton.IsEnabled = value;
        }
        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable the controls so they don't change during execution
            SetActive(false);
            DataGridDisplay.ItemsSource = null; // Clear the grid

            LogTextBlock.Text =
                $"Executing {ComboBoxAlgorithm.SelectedValue} for {NumberRuns.SelectedValue} runs up to {ComboBoxDataSize.SelectedValue}.  The data is initialized in a(n) {ComboBoxInitialSortingPolicy.SelectedValue} order\n\n";

            int dataAmount = 0;
            int incrementAmount = MaximumValue / NumberTestPoints;
            runs = new List<AnalysisRun>();
            for (int i = 0; i < NumberTestPoints; i++)
            {
                dataAmount += incrementAmount;
                runs.Add(new AnalysisRun(dataAmount, InitialOrderSelected, GetObjectiveFunction()));
            }

            foreach (AnalysisRun run in runs)
            {
                LogTextBlock.Text += $"Starting to sort {run.NumberItems} items. Is it sorted?= {run.IsSorted()}\n";
                await run.RunAlgorithmAsyncrhonousForTimer();
                LogTextBlock.Text += $"Completed sorting {run.NumberItems} items in {run.ExecutionTime} ms. Is it sorted?= {run.IsSorted()}\n";
            }

            //await Task.Run(() => Parallel.ForEach(runs,
            //    (run) => run.RunAlgorithmForTimerSynchronous()));
            UpdateTable();

            MessageBox.Show("Runs completed");
            SetActive(true);
        }
    }

    public class AnalysisRun
    {
        public int NumberItems { get; set; }
        public long ExecutionTime { get; set; }
        public long NumberOperations { get; set; }
        private IntegerArrayWithEvents.InitialOrdering Order { get; set; }
        private IntegerArrayWithEvents _data;
        private AbstractAlgorithm _algorithm;

        public AnalysisRun(int numberItems, IntegerArrayWithEvents.InitialOrdering ordering, AbstractAlgorithm algorithm)
        {
            _algorithm = algorithm;
            NumberItems = numberItems;
            Order = ordering;
            _data = new IntegerArrayWithEvents(numberItems, numberItems);
            _data.MixNumbersWithoutEvents(Order);
        }

        public bool IsSorted() => _data.TestIfIsSortedAscending();

        public async Task RunAlgorithmAsyncrhonousForTimer()
        {
            //System.Diagnostics.Debug.WriteLine("Running with " + _data.Count + ".  Sorted? " + _data.TestIfIsSortedAscending());
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            NumberOperations = await _algorithm.Run(_data);
            sw.Stop();
            //System.Diagnostics.Debug.WriteLine("Completed run with " + _data.Count + " in " + ExecutionTime + ".  Sorted? " + _data.TestIfIsSortedAscending());            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);
            
            ExecutionTime = sw.ElapsedMilliseconds;
}
       
        public void RunAlgorithmForTimerSynchronous()
        {
            System.Diagnostics.Debug.WriteLine("Running with " + _data.Count + ".  Sorted? " + _data.TestIfIsSortedAscending());
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            NumberOperations = _algorithm.RunSynchronous(_data);
            sw.Stop();
            ExecutionTime = sw.ElapsedMilliseconds;

            System.Diagnostics.Debug.WriteLine("Completed run with " + _data.Count + " in " + ExecutionTime + ".  Sorted? " + _data.TestIfIsSortedAscending());            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);

            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);
        }
    }
}
