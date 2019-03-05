using System.Windows;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelperUI : Window
    {
        SplitAndMerge splitMergeWindow;

        public HelperUI()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(HelperUI_Loaded);
        }

        void HelperUI_Loaded(object sender, RoutedEventArgs e)
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
