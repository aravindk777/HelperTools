using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Helper_UI
{
    /// <summary>
    /// Interaction logic for SplitAndMerge.xaml
    /// </summary>
    public partial class SplitAndMerge : Window
    {
        public delegate void UpdateOutputResultCallback(string searchChar, string splitChar, string source, bool? styleString, bool? addSpaece);

        #region Properties
        string SearchChar { get; set; }
        string SplitChar { get; set; }
        string SourceInfo { get; set; }
        bool? DecorateOutputAsString { get; set; }
        bool? AddSpaceBtwnResults { get; set; }

        public Dictionary<string, string> SourceList {
            get{
                var dict = new Dictionary<string, string>();
                dict.Add("NewLine", "\r\n");
                dict.Add("Comma (,)", ",");
                dict.Add("Pipe (|)", "|");
                dict.Add("Semicolon (;)", ";");

                return dict;
            }
        }
        #endregion

        public SplitAndMerge()
        {
            InitializeComponent();
            //lstSearchChar.DataContext = SourceList;
            //DataContext = SourceList;
            this.Loaded += SplitAndMerge_Loaded;
        }

        private void SplitAndMerge_Loaded(object sender, RoutedEventArgs e)
        {
            cbxSplitChar.TextInput += CbxSplitChar_TextInput;
            cbxSplitChar.SelectionChanged += CbxSplitChar_SelectionChanged;
            TxtSplitChar.TextChanged += TxtSplitChar_TextChanged;
        }

        private void CbxSplitChar_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // chkNeedSpace.Content = "Space after " + (cbxSplitChar.SelectedIndex >= 0 ? cbxSplitChar.SelectedValue.ToString() : cbxSplitChar.Text) + " ?";
            TxtSplitChar.Text = cbxSplitChar.SelectedIndex >= 0 ? cbxSplitChar.SelectedValue.ToString() : cbxSplitChar.Text;
        }

        void TxtSplitChar_TextChanged(object sender, TextChangedEventArgs e)
        {
            chkNeedSpace.Content = "Space after " + TxtSplitChar.Text + " ?";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtOutput.Text = "working...";
            TxtOutput.Cursor = System.Windows.Input.Cursors.Wait;

            SearchChar = cbxSearchChar.SelectedIndex >=0 ? cbxSearchChar.SelectedValue.ToString() : cbxSearchChar.Text;
            SplitChar = cbxSplitChar.SelectedIndex >= 0 ? cbxSplitChar.SelectedValue.ToString() : cbxSplitChar.Text;
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
            TxtOutput.Cursor = System.Windows.Input.Cursors.IBeam;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxtOutput.Text);
        }

        private void CbxSplitChar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //chkNeedSpace.Content = "Space after " + (cbxSplitChar.SelectedIndex >= 0 ? cbxSplitChar.SelectedValue.ToString() : cbxSplitChar.Text) + " ?";
            TxtSplitChar.Text = cbxSplitChar.SelectedIndex >= 0 ? cbxSplitChar.SelectedValue.ToString() : cbxSplitChar.Text;
        }
    }
}
