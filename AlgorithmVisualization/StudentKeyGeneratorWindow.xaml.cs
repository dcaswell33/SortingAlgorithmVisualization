using Microsoft.Win32;
using System;
using System.Text;
using System.Windows;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for StudentKeyGeneratorWindow.xaml
    /// </summary>
    public partial class StudentKeyGeneratorWindow : Window
    {
        public StudentKeyGeneratorWindow()
        {
            InitializeComponent();
        }

        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (saveFileDialog.ShowDialog() == true)
                FileNameSaveTextBox.Text = saveFileDialog.FileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = studentEmailTextBox.Text;
            string[] items = text.Split(new string[] { ",", "\r", "\n", " " }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();
            foreach (string item in items)
            {
                Type[] typeList = LostOrdering.SetupLostSortOrdering(LostOrdering.userNameAsSHA(item));
                sb.Append(item);
                foreach (Type t in typeList)
                {
                    sb.Append(',').Append(t.Name);
                }
                sb.AppendLine();
            }
            System.Diagnostics.Debug.WriteLine(sb.ToString());

            if (saveFileDialog.FileName.Length > 0)
                System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());
        }
    }
}
