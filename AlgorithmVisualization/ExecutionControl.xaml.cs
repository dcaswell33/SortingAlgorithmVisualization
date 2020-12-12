using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.ComponentModel;
using System.Globalization;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for ExecutionControl.xaml
    /// </summary>
    public partial class ExecutionControl : UserControl
    {
        public ExecutionControl()
        {
            InitializeComponent();
            DataContext = new SampleContext();
            ((SampleContext) DataContext).IsPlaying = false;
            ((SampleContext) DataContext).PropertyChanged += ExecutionControl_PropertyChanged;
        }

        public void ResetExecution()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(ResetExecution);
                return;
            }
            ((SampleContext)DataContext).IsPlaying = false;
        }

        private void ExecutionControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (((SampleContext) DataContext).IsPlaying)
            {
                PlayButton.Content = "Pause";
            }
            else
            {
                PlayButton.Content = "Play";
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            var context = DataContext as SampleContext;

            if (context == null)
                return;

            context.IsPlaying = !context.IsPlaying;
            OnStartStopChanged(new StartStopEventArgs(context.IsPlaying));
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnSpeedChanged(new SpeedChangedEventArgs((int)SpeedSlider.Value));
        }
        
        public delegate void SpeedChangedEventHandler(SpeedChangedEventArgs e);
        public event SpeedChangedEventHandler SpeedChanged;

        protected void OnSpeedChanged(SpeedChangedEventArgs e)
        {
            SpeedChanged?.Invoke(e);
        }


        public delegate void StartStopEventHandler(StartStopEventArgs e);
        public event StartStopEventHandler StartStopChanged;

        protected void OnStartStopChanged(StartStopEventArgs e)
        {
            StartStopChanged?.Invoke(e);
        }
    }

    public class SpeedChangedEventArgs: EventArgs
    {
        public int Speed { get; set; }
        public SpeedChangedEventArgs(int value)
        {
            Speed = value;
        }
    }

    public class StartStopEventArgs : EventArgs
    {
        public bool Play { get; set; }
        public StartStopEventArgs(bool value)
        {
            Play = value;
        }
    }

    public class SampleContext : INotifyPropertyChanged
    {
        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying == value)
                    return;

                _isPlaying = value;

                OnPropertyChanged("IsPlaying");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static BoolToVisibilityConverter _instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Hidden;

            if (parameter != null)
                visibility = (Visibility)parameter;

            return visibility == Visibility.Visible
                ? (((bool)value) ? Visibility.Visible : Visibility.Hidden)
                : (((bool)value) ? Visibility.Hidden : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new BoolToVisibilityConverter());
        }
    }
}