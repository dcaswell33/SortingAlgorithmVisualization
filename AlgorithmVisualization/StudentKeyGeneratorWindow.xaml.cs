using AlgorithmVisualization.Algorithms;
using Microsoft.Win32;
using System;
using System.Text;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

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
        
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private void btnLoadXLSX_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = "Excel files (*.xls, .xlsx)|*.xlsx; *.xls|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                FileNameLoadTextBox.Text = openFileDialog.FileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string text = studentEmailTextBox.Text;
            //string[] items = text.Split(new string[] { ",", "\r", "\n", " " }, StringSplitOptions.RemoveEmptyEntries);

            //StringBuilder sb = new StringBuilder();
            //foreach (string item in items)
            //{
            //    Type[] typeList = LostOrdering.SetupLostSortOrdering(LostOrdering.userNameAsSHA(item));
            //    sb.Append(item);
            //    foreach (Type t in typeList)
            //    {
            //        sb.Append(',').Append(t.Name);
            //    }
            //    sb.AppendLine();
            //}
            //System.Diagnostics.Debug.WriteLine(sb.ToString());

            //if (saveFileDialog.FileName.Length > 0)
            //    System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());

            Parse_Student_Quiz_Submissions(openFileDialog.FileName);
        }

        private void Parse_Student_Quiz_Submissions(string inputFile)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook = null;
            Excel.Worksheet xlWorkSheet = null;
            object misValue = System.Reflection.Missing.Value;

            StringBuilder sb = new StringBuilder();
            StringBuilder results = new StringBuilder();

            xlApp = new Excel.Application();
            try
            {
                xlWorkBook = xlApp.Workbooks.Open(inputFile,
                     Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Excel.Range range = xlWorkSheet.UsedRange;
                int numRows = range.Rows.Count;
                int numCols = range.Columns.Count;

                int sortIndex = -1;
                int questionIndex = -1;
                int numSortsCorrect = 0;

                Type[] typeList = null;
                AbstractAlgorithm algorithm = null;

                for (int rowIndex = 2; rowIndex <= numRows; rowIndex++)
                {
                    int score = 0;
                    sortIndex = -1;
                    numSortsCorrect = 0;

                    for (int colIndex = 4; colIndex <= numCols; colIndex++)
                    {
                        // Filter out unnecessary items
                        string header = ((string)(range.Cells[1, colIndex] as Excel.Range).Value2).ToLower();
                        if (!header.Contains("points") && !header.Contains("feedback"))
                        {
                            questionIndex = -1;

                            string cellValue = ((string)(range.Cells[rowIndex, colIndex] as Excel.Range).Value2).ToLower();

                            if (header.Contains("email"))
                            {
                                typeList = LostOrdering.SetupLostSortOrdering(LostOrdering.userNameAsSHA(cellValue));
                                sb.Append(cellValue).Append("\n");
                                results.Append(cellValue);
                            }

                            if (header.Contains("randomly"))
                            {
                                questionIndex = 0;
                                sortIndex++;
                                sb.Append("Sort=").Append(typeList[sortIndex].Name).Append("{");
                                results.Append(",").Append(typeList[sortIndex].Name);
                                algorithm = (Algorithms.AbstractAlgorithm)Activator.CreateInstance(typeList[sortIndex]);
                            }
                            if (header.Contains("ascending")) questionIndex = 1;
                            if (header.Contains("descending")) questionIndex = 2;

                            if (questionIndex >= 0)
                            {
                                if (cellValue.Contains(algorithm.QuestionAnswers[questionIndex]))
                                {
                                    // correct answer
                                    score += 1;
                                    results.Append(",1");
                                }
                                else
                                {
                                    // incorrect answer
                                    results.Append(",0");
                                }
                            }

                            if (header.Contains("what sorting algorithm")) 
                            {
                                questionIndex = 4;

                                // Reference the last item in the questions to account for multiple options for Q4
                                if (cellValue.Contains(algorithm.QuestionAnswers[algorithm.QuestionAnswers.Length - 1]))
                                {
                                    // correct answer
                                    score += 1;
                                    results.Append(",1");
                                    numSortsCorrect += 1;
                                }
                                else
                                {
                                    // incorrect answer
                                    results.Append(",0");
                                }
                            }

                            if (header.Contains("attributes you think is appropriate"))
                            {
                                questionIndex = 3;
                                string[] items = cellValue.Split(';');
                                int numAccurate = 0;

                                // Count how many are accurate
                                for (int i = 3; i < (algorithm.QuestionAnswers.Length - 1); i++)
                                {
                                    if (cellValue.Contains(algorithm.QuestionAnswers[i])) numAccurate++;
                                }

                                if (items.Length == 0) results.Append(",0");  // no answer = no points
                                else if (items.Length == algorithm.QuestionAnswers.Length - 4 && numAccurate == items.Length - 1)
                                {
                                    // right number of answers and all are in the list then max points
                                    score += 2;
                                    results.Append(",2");
                                }
                                else
                                {
                                    score += 1;
                                    results.Append(",1");
                                }
                            }
                            
                            if (questionIndex >= 0)
                            {
                                sb.Append(questionIndex + 1).Append(":").Append(cellValue);
                                if (questionIndex != 4)
                                    sb.Append(", ");
                                else sb.Append("}\n");
                            }                            
                        }
                    }
                    //results.Append("\nTotal:").Append(score).Append("    #Sorts Correct:").Append(numSortsCorrect).Append("\n");
                    results.Append(",").Append(score).Append("\n");
                }

                xlWorkBook.Close(false, null, null);
                xlApp.Quit();
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
            finally
            {
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }

            System.IO.File.WriteAllText("StudentAnswers.txt", sb.ToString());
            System.IO.File.WriteAllText("StudentScores.txt", results.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            System.Diagnostics.Debug.WriteLine(results.ToString());
        }

        private void releaseObject(object obj)
        {
            if (obj == null) return;
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
