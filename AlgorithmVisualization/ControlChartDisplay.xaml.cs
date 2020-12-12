using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for ControlChartDisplay.xaml
    /// </summary>
    public partial class ControlChartDisplay : UserControl
    {
        private const int CHARTBORDER = 10;
        public ControlChartDisplay()
        {
            InitializeComponent();
        }

        private IntegerArrayWithEvents _data;
        private double _barWidth = 0;
        private double _barHeightScalar = 0;
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private Rectangle _tempRectangle;
        private List<SolidColorBrush> _brushes;

        public void Setup(IntegerArrayWithEvents data)
        {
            if (_data != null && _data.IsElementExaminedEventHandlerRegistered(new IntegerArrayWithEvents.ElementExaminedEventHandler(Data_ElementExamined)))
            {
                data.ElementChanged -= Data_ElementChanged;
                data.ElementExamined -= Data_ElementExamined;
                data.ElementsSwapped -= Data_ElementsSwapped;
                data.AlgorithmFinished -= Data_AlgorithmFinished;
                data.TempVariableChanged -= Data_TempVariableChanged;
            }
            _data = data;
            data.ElementChanged += Data_ElementChanged;
            data.ElementExamined += Data_ElementExamined;
            data.ElementsSwapped += Data_ElementsSwapped;
            data.AlgorithmFinished += Data_AlgorithmFinished;
            data.TempVariableChanged += Data_TempVariableChanged;

            _barWidth = (DisplayChart.ActualWidth - CHARTBORDER) / _data.Count;
            _barHeightScalar = (DisplayChart.ActualHeight - CHARTBORDER) / _data.MaxValue;

            BuildBrushList();

            DrawData();
        }

        public void Refresh()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(Refresh);
                return;
            }
            UnHighlightElements();
            DrawData();
        }

        private void Data_AlgorithmFinished(EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(() => Data_AlgorithmFinished(e)));
                return;
            }
            UnHighlightElements();
            DrawData();
        }

        private void BuildBrushList()
        {
            _brushes = new List<SolidColorBrush>(_data.MaxValue+1);
            for (int i = 0; i <= _data.MaxValue; i++)
            {
                _brushes.Add(new SolidColorBrush(GetBlueGreenColorFromPosition(i)));
            }
        }

        private System.Windows.Media.Color GetBlueGreenColorFromPosition(int value)
        {
            double position = (double)value / _data.MaxValue;
            if (position > 1.0) position = 1.0;
            if (position < 0) position = 0;

            return System.Windows.Media.Color.FromArgb(255, 0, Convert.ToByte(255 * position), Convert.ToByte(255 * (1 - position)));
        }

        private void DrawData()
        {
            DisplayChart.Children.Clear();
            TempVariableChart.Children.Clear();
            // Draw the primary data
            _rectangles.Clear();
            for (int i = 0; i < _data.Count; i++)
            {
                Rectangle rect = DrawRectangle(_data.GetValueWithoutEvents(i));

                // Add Rectangle to the Grid.
                Canvas.SetLeft(rect, _barWidth * i);
                DisplayChart.Children.Add(rect);
                _rectangles.Add(rect);
            }

            // Draw the temp variable
            _tempRectangle = DrawRectangle(0);
            Canvas.SetLeft(_tempRectangle, (TempVariableChart.ActualWidth - _barWidth) / 2);
            TempVariableChart.Children.Add(_tempRectangle);
        }

        private Rectangle DrawRectangle(int value)
        {
            // Create a Rectangle
            Rectangle rect = new Rectangle();
            rect.Height = value * _barHeightScalar;
            rect.Width = _barWidth;
                        
            // Set Rectangle's width and color
            rect.StrokeThickness = 4;
            rect.Stroke = _brushes[value];

            // Fill rectangle with blue color
            rect.Fill = _brushes[value];

            Canvas.SetBottom(rect, 5);

            return rect;
        }

        private void UpdateRectangle(int index)
        {
            if (!DisplayChart.Dispatcher.CheckAccess())
            {
                DisplayChart.Dispatcher.Invoke(new Action(() => UpdateRectangle(index)));
            }
            else
            {
                int val = _data.GetValueWithoutEvents(index);
                _rectangles[index].Height = val * _barHeightScalar;
                _rectangles[index].Fill = _brushes[val];
                _rectangles[index].Stroke = _brushes[val];
            }
        }

        private void UpdateTempRectangle(int value)
        {
            if (!DisplayChart.Dispatcher.CheckAccess())
            {
                DisplayChart.Dispatcher.Invoke(new Action(() => UpdateTempRectangle(value)));
            }
            else
            {
                _tempRectangle.Height = value * _barHeightScalar;
                _tempRectangle.Fill = _brushes[value];
                _tempRectangle.Stroke = _brushes[value];
            }
        }

        private List<int> _highlightedRectangleIndices = new List<int>();
        private SolidColorBrush _highlightBrushStroke = new SolidColorBrush(Colors.Yellow);

        private void UnHighlightElements()
        {
            if (!DisplayChart.Dispatcher.CheckAccess())
            {
                DisplayChart.Dispatcher.Invoke(new Action(() => UnHighlightElements()));
            }
            else
            {
                foreach (int index in _highlightedRectangleIndices)
                {
                    int val = _data.GetValueWithoutEvents(index);
                    _rectangles[index].Fill = _brushes[val];
                    _rectangles[index].Stroke = _brushes[val];
                }
                _highlightedRectangleIndices.Clear();
            }
        }

        private void HighlightElement(int index)
        {
            if (!DisplayChart.Dispatcher.CheckAccess())
            {
                DisplayChart.Dispatcher.Invoke(new Action(() => HighlightElement(index)));
            }
            else
            {
                _highlightedRectangleIndices.Add(index);
                _rectangles[index].Fill = _highlightBrushStroke;
                _rectangles[index].Stroke = _highlightBrushStroke;
            }
        }

        private void Data_ElementsSwapped(ElementsSwappedEventArgs e)
        {
            UpdateRectangle(e.FirstIndex);
            UpdateRectangle(e.SecondIndex);
        }

        private void Data_ElementExamined(ElementExaminedEventArgs e)
        {
            UnHighlightElements();
            HighlightElement(e.Index);
        }

        private void Data_ElementChanged(ElementChangedEventArgs e)
        {
            UpdateRectangle(e.Index);
        }

        private void SolutionChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_data != null)
            {
                _barWidth = (DisplayChart.ActualWidth - CHARTBORDER) / _data.Count;
                _barHeightScalar = (DisplayChart.ActualHeight - CHARTBORDER) / _data.MaxValue;

                Rectangle rect;
                for (int i = 0; i < _rectangles.Count; i++)
                {
                    int val = _data.GetValueWithoutEvents(i);
                    rect = _rectangles[i];
                    rect.Height = val * _barHeightScalar;
                    rect.Width = _barWidth;
                    Canvas.SetBottom(rect, 5);
                    Canvas.SetLeft(rect, _barWidth * i);
                }

                //TempRectangle
                _tempRectangle.Height = _data.TempInteger * _barHeightScalar;
                _tempRectangle.Width = _barHeightScalar;
                Canvas.SetLeft(_tempRectangle, (TempVariableChart.ActualWidth - _barWidth) / 2);
                 
            }
        }

        private void Data_TempVariableChanged(ElementChangedEventArgs e)
        {
            UpdateTempRectangle(e.Value);
        }

    }
}
