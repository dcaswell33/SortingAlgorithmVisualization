using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

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
        private bool _showAllSorts = false;

        private Type[] _orderedSortingAlgorithms = null;

        public MainWindow()
        {
            InitializeComponent();

            UsernameEntry usernameEntry = new UsernameEntry();
            if (usernameEntry.ShowDialog() ?? true)
            {
                string userNameAsSHA = LostOrdering.userNameAsSHA(usernameEntry.userName);
                Debug.WriteLine(userNameAsSHA);
                if (userNameAsSHA == "fe8f82dd4271c78f269b8e8dc20b66bafda6775f55daf84a48d8ac7199c3276c")
                {
                    // Show all of the sorts
                    _showAllSorts = true;
                    StudentKeyGeneratorWindow generator = new StudentKeyGeneratorWindow();
                    generator.Show();
                }                
                else
                {
                    _orderedSortingAlgorithms = LostOrdering.SetupLostSortOrdering(userNameAsSHA);
                }
            }
            else
            {
                this.Close();
            }

            SetupObjectivesComboBox();

            _data = new IntegerArrayWithEvents(Properties.Settings.Default.NumberItems,
                Properties.Settings.Default.MaxIntSize);
            _data.MixNumbersWithoutEvents(InitialOrderSelected);
            _data.AlgorithmFinished += _data_AlgorithmFinished;
        }

        private void MainWindow1_ContentRendered(object sender, EventArgs e)
        {
            GraphicVisualization.Setup(_data);
        }

        public void ToggleCodeWindow(bool showCodeWindow)
        {
            if (showCodeWindow)
            {
                GraphicVisualization.SetValue(Grid.ColumnSpanProperty, 1);
                DataGridDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                GraphicVisualization.SetValue(Grid.ColumnSpanProperty, 2);
                DataGridDisplay.Visibility = Visibility.Hidden;
            }
        }

        private void _data_AlgorithmFinished(EventArgs e)
        {
            ExecutionControl.ResetExecution();
            SetActive(true);
        }

        #region Algorithm Parameterization

        #region Algorithm ComboBox GUI
        private Algorithms.AbstractAlgorithm GetObjectiveFunction()
        {
            return (Algorithms.AbstractAlgorithm)Activator.CreateInstance((Type)((ComboBoxItem)ComboBoxAlgorithm.SelectedItem).Tag);
        }

        private void SetupObjectivesComboBox()
        {

            if (_showAllSorts)
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
            }
            else
            {
                for (int index = 0; index < _orderedSortingAlgorithms.Length; index++)
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Content = "Lost Sort " + index,
                        Tag = _orderedSortingAlgorithms[index]
                    };
                    ComboBoxAlgorithm.Items.Add(newItem);
                }
            }
            ComboBoxAlgorithm.SelectedIndex = 0;
        }

        private void comboBoxAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] codeString = GetObjectiveFunction().CodeString();

            if (codeString.Length == 1 || _showAllSorts == false) { ToggleCodeWindow(false);  }
            else {
                ToggleCodeWindow(true);
                DataGridDisplay.GridLinesVisibility = DataGridGridLinesVisibility.None;
                DataGridDisplay.HeadersVisibility = DataGridHeadersVisibility.None;
                DataGridDisplay.ItemsSource = GetObjectiveFunction().CodeString().Select(x => new { Value = x }).ToList();
            }

            ResetVisualization();
        }

        private void ComboBoxInitialSortingPolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetVisualization();
        }

        private void ComboBoxDataSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetVisualization();
        }
        #endregion

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

        #endregion

        #region Algorithm Visualization

        private void ResetVisualization()
        {
            if (_data != null)
            {
                if (_algorithm != null) _algorithm.AllowRun = false;
                int numData = NumDataElementsSelected;
                //Ensure we don't create too much data for the visualization
                if (numData > 500) numData = 500;

                if (_data.Count != numData)
                    _data.ReInitializeData(numData, numData);
                _data.MixNumbersWithoutEvents(InitialOrderSelected);

                GraphicVisualization?.Setup(_data);
            }
        }

        private void ButtonMixData(object sender, RoutedEventArgs e)
        {
            ResetVisualization();
        }

        private void executionControl_SpeedChanged(SpeedChangedEventArgs e)
        {
            _data.DelayMs = (e.Speed * Properties.Settings.Default.MaxDelay) / 10;
        }

        private void executionControl_StartStopChanged(StartStopEventArgs e)
        {
            if (e.Play)
            {
                SetActive(false);
                RunAlgorithmForTimer();
            }
            else
            {
                // Stop
                _algorithm.AllowRun = false;
            }
        }

        private async void RunAlgorithmForTimer()
        {
            System.Diagnostics.Stopwatch sw = Stopwatch.StartNew();
            _algorithm = GetObjectiveFunction();
            _algorithm.AllowRun = true;
            await _algorithm.Run(_data);

            sw.Stop();
            UpdateLastExecutionTime(sw.ElapsedMilliseconds);
        }

        private void UpdateLastExecutionTime(long value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => UpdateLastExecutionTime(value));
                return;
            }

            RunTimeTextBlock.Text="Execution Completed in " + value + " ms";
        }


        #endregion

        #region Analysis

        private int MaximumValue => Convert.ToInt32(ComboBoxDataSize.SelectedValue.ToString());
        private int NumberTestPoints => Convert.ToInt32(NumberRuns.SelectedValue.ToString());

        private void UpdateTable()
        {
            DataGridAnalysisDisplay.ItemsSource = runs;
        }

        private List<AnalysisRun> runs;

        private void SetActive(bool value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(() => SetActive(value)));
                return;
            }
            else
            {
                ComboBoxInitialSortingPolicy.IsEnabled = value;
                ComboBoxDataSize.IsEnabled = value;
                ComboBoxInitialSortingPolicy.IsEnabled = value;
                ComboBoxAlgorithm.IsEnabled = value;
                NumberRuns.IsEnabled = value;
                RunButton.IsEnabled = value;
                MixButton.IsEnabled = value;
            }
        }
        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable the controls so they don't change during execution
            SetActive(false);
            DataGridAnalysisDisplay.ItemsSource = null; // Clear the grid

            int currentDelay = _data.DelayMs;
            _data.DelayMs = 0;
            ExecutionControl.IsEnabled = false;

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
                await run.RunAlgorithmAsynchronousForTimer();
                LogTextBlock.Text += $"Completed sorting {run.NumberItems} items in {run.ExecutionTime} ms. Is it sorted?= {run.IsSorted()}\n";
            }

            //await Task.Run(() => Parallel.ForEach(runs,
            //    (run) => run.RunAlgorithmForTimerSynchronous()));
            UpdateTable();

            _data.DelayMs = currentDelay;
            ExecutionControl.IsEnabled = true;

            MessageBox.Show("Runs completed");
            SetActive(true);
        }
        #endregion

    }
    public class Converter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return object.Equals(value, true) ? "*" : "Auto";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
    