using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RunMyQuery;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelperUI : Window
    {
        SplitAndMerge splitMergeWindow;
        RunMyQuery.MainWindow runQueryWindow;
        AgentView agtView;

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

        private void BtnRunMyQuery_Click(object sender, RoutedEventArgs e)
        {
            if (runQueryWindow == null)
                runQueryWindow = new RunMyQuery.MainWindow();

            runQueryWindow.Show();
        }

        private void BtnAgentView_Click(object sender, RoutedEventArgs e)
        {
            if (agtView == null)
                agtView = new AgentView();

            agtView.Show();
        }


    }
}
