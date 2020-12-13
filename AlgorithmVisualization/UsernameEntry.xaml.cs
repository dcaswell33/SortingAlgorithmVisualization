using System.Windows;

namespace AlgorithmVisualization
{
    /// <summary>
    /// Interaction logic for UsernameEntry.xaml
    /// </summary>
    public partial class UsernameEntry : Window
    {
        public UsernameEntry()
        {
            InitializeComponent();
        }

        public string userName => usernameTextBox.Text;

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Length != 0) 
                this.DialogResult = true;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
