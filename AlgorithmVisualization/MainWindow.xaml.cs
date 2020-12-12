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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntegerArrayWithEvents _data;
        private Algorithms.AbstractAlgorithm _algorithm;

        private List<ExecutionTiming> TimeOfExecutions { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetupObjectivesComboBox();

            //TimeOfExecutions = new List<ExecutionTiming>();

            // Tests the data grid
            //TimeOfExecutions.Add(new ExecutionTiming(50,10001));
            //TimeOfExecutions.Add(new ExecutionTiming(100, 100201));
            //DataGridDisplay.ItemsSource = TimeOfExecutions;

            _data = new IntegerArrayWithEvents(Properties.Settings.Default.NumberItems, 
                Properties.Settings.Default.MaxIntSize);
            _data.MixNumbersWithoutEvents(InitialOrderSelected);
            _data.AlgorithmFinished += _data_AlgorithmFinished;
        }

        private void _data_AlgorithmFinished(EventArgs e)
        {
            ExecutionControl.ResetExecution();
        }

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

        private int NumDataElementsSelected => Convert.ToInt32(ComboBoxDataSize.SelectedValue.ToString());

        private void ButtonMixData(object sender, RoutedEventArgs e)
        {
            int numData = NumDataElementsSelected;
            if (_data.Count != numData)
                _data.ReInitializeData(numData, numData);
            _data.MixNumbersWithoutEvents(InitialOrderSelected);
            if (_algorithm != null) _algorithm.AllowRun = false;

            GraphicVisualization?.Setup(_data);
            //GraphicVisualization.Refresh();
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

        private void MainWindow1_ContentRendered(object sender, EventArgs e)
        {
            GraphicVisualization.Setup(_data);
        }

        private void executionControl_SpeedChanged(SpeedChangedEventArgs e)
        {
            _data.DelayMs = (e.Speed * Properties.Settings.Default.MaxDelay) / 10;
        }

        private void executionControl_StartStopChanged(StartStopEventArgs e)
        {
            if (e.Play)
            {
                // Start
                //_algorithm = GetObjectiveFunction();
                //_algorithm.Run(_data);
                RunAlgorithmForTimer();
            }
            else
            {
                // Stop
                _algorithm.AllowRun = false;
            }
        }

        //private long LastExecutionTime { get; set; }

        private void UpdateLastExecutionTime(long value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateLastExecutionTime(value));
                return;
            }

            //LastExecutionTime = value;
            RunTimeTextBlock.Text="Execution Completed in " + value + " ms";
        }

        private async void RunAlgorithmForTimer()
        {
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            _algorithm = GetObjectiveFunction();
            await _algorithm.Run(_data);
            sw.Stop();
            UpdateLastExecutionTime(sw.ElapsedMilliseconds);
            //MessageBox.Show("Execution Completed in " + sw.ElapsedMilliseconds);
        }

        private void comboBoxAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridDisplay.GridLinesVisibility = DataGridGridLinesVisibility.None;
            DataGridDisplay.HeadersVisibility = DataGridHeadersVisibility.None;
            DataGridDisplay.ItemsSource = GetObjectiveFunction().CodeString().Select(x => new { Value = x }).ToList();
            //PseudoCodeTextBlock.Text = "";
            //string[] code = GetObjectiveFunction().CodeString();
            //int highlightedRow = -1;
            //int index = 0;
            //foreach (string s in code)
            //{
            //    if (index == highlightedRow)
            //    {
            //        PseudoCodeTextBlock.Inlines.Add(new Run(s + "\n") { Background = Brushes.Yellow });
            //    }
            //    else
            //    {
            //        PseudoCodeTextBlock.Inlines.Add(s + "\n");
            //    }
            //    index++;
            //}
        }

        private async Task<long> RunAlgorithmForTime(AbstractAlgorithm algorithm, IntegerArrayWithEvents data)
        { 
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            await algorithm.Run(data);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private void ButtonAnalyze_OnClick(object sender, RoutedEventArgs e)
        {
            SortingAlgorithmAnalysis analyzer = new SortingAlgorithmAnalysis();
            analyzer.Show();
            //if (MessageBox.Show("Warning: This will take some time to run.  Do you want to continue?", "Warning",
            //        MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            //{

            //    //IntegerArrayWithEvents data = new IntegerArrayWithEvents(dataLength, dataLength);
            //    //data.MixNumbersWithoutEvents(InitialOrderSelected);
            //    //UpdateLastExecutionTime(sw.ElapsedMilliseconds);
            //}
        }
    }

}
    