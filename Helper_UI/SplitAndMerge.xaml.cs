using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for SplitAndMerge.xaml
    /// </summary>
    public partial class SplitAndMerge : Window
    {
        public delegate void UpdateOutputResultCallback(string searchChar, string splitChar, string source, bool? styleString, bool? addSpaece);
        string SearchChar { get; set; }
        string SplitChar { get; set; }
        string SourceInfo { get; set; }
        bool? DecorateOutputAsString { get; set; }
        bool? AddSpaceBtwnResults { get; set; }

        public SplitAndMerge()
        {
            InitializeComponent();
            TxtSplitChar.TextChanged += new TextChangedEventHandler(TxtSplitChar_TextChanged);
        }

        void TxtSplitChar_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkNeedSpace.Content = "Space after " + TxtSplitChar.Text + " ?";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtOutput.Text = "working...";

            SearchChar = TxtSearchChar.Text;
            SplitChar = TxtSplitChar.Text;
            SourceInfo = TxtSource.Text;
            DecorateOutputAsString = chkIsString.IsChecked;
            AddSpaceBtwnResults = chkNeedSpace.IsChecked;

            Thread tOutput = new Thread(new ThreadStart(DoWork));
            tOutput.Start();
        }

        void DoWork()
        {
            Thread.Sleep(500);
            TxtOutput.Dispatcher.Invoke(
                new UpdateOutputResultCallback(ProcessData), 
                SearchChar, SplitChar, SourceInfo, DecorateOutputAsString, AddSpaceBtwnResults
                );
        }

        void ProcessData(string searchChar, string splitChar, string source, bool? styleString, bool? addSpaece)
        {
            var searchString = searchChar.Equals("\\r\\n") ? Environment.NewLine : searchChar;
            splitChar = splitChar.Contains("\\r\\n") ? splitChar.Replace("\\r\\n", Environment.NewLine) : splitChar;
            var outputStrings = source.Split(new string[] { searchString }, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).ToList();

            //outputString = TxtSource.Text.Replace(searchString, splitChar);

            if (styleString.HasValue && styleString.Value)
            {
                List<string> formattedData = new List<string>(outputStrings.Count);
                outputStrings.ForEach(s => formattedData.Add(string.Format("{0}{1}{0}", "'", s)));
                outputStrings = formattedData;
            }

            string finalReult = string.Empty;
            string spaceText = string.Empty;
            if (addSpaece.HasValue && addSpaece.Value)
                spaceText = " ";
            outputStrings.ForEach(formattedText => { finalReult += string.Format("{0}{1}{2}", formattedText, splitChar, spaceText); });

            TxtOutput.Text = finalReult.Remove(finalReult.LastIndexOf(splitChar));
        }
    }
}
