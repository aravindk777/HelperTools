using Helper_UI;
using System.Windows;

namespace HelperUI.Wpf.NetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SplitAndMerge? splitMergeWindow = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEncode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSplitWindow_Click(object sender, RoutedEventArgs e)
        {
            if (splitMergeWindow == null)
                splitMergeWindow = new SplitAndMerge();

            splitMergeWindow.Show();
        }
    }
}