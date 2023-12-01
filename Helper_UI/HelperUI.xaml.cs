using Helper_UI.Encryptor;
using Helper_UI.RunAs;
using System.Windows;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelperUI : Window
    {
        SplitAndMerge splitMergeWindow;
        RunProgramAs runAsWindow;

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

        private void BtnRunAs_Click(object sender, RoutedEventArgs e)
        {
            runAsWindow = new RunProgramAs();
            runAsWindow.Show();   
        }

        private void BtnEncryptor_Click(object sender, RoutedEventArgs e)
        {
            var encryptorWindow = new Encryptors();
            encryptorWindow.Show();
        }
    }
}
