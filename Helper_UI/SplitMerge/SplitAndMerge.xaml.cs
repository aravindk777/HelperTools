using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Dictionary<string, string> SourceList
        {
            get
            {
                var dict = new Dictionary<string, string>();
                dict.Add("NewLine", "\r\n");
                dict.Add("Comma (,)", ",");
                dict.Add("Pipe (|)", "|");
                dict.Add("Semicolon (;)", ";");

                return dict;
            }
        }

        public Dictionary<string, int> PageSizes
        {
            get
            {
                var dict = new Dictionary<string, int>();
                dict.Add("10", 10);
                dict.Add("50", 50);
                dict.Add("100", 100);
                dict.Add("500", 500);
                dict.Add("1000", 1000);
                return dict;
            }
        }

        private IEnumerable<object> _results;

        private int _start = 0;
        public int Start { get { return _start + 1; } }

        private int _itemCount = 10;
        private int _totalPages;

        public event PropertyChangedEventHandler PropertyChanged;

        public int TotalItems { get { return _results != null ? _results.Count() : 0; } }
        public int End { get { return _start + _itemCount < TotalItems ? _start + _itemCount : TotalItems; } }


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
            chkPaginated.Checked += ChkPaginated_Checked;
            chkPaginated.Unchecked += ChkPaginated_Checked;
            //PaginationControls.Visibility = Visibility.Hidden;
        }

        private void ChkPaginated_Checked(object sender, RoutedEventArgs e)
        {
            //if (chkPaginated.IsChecked == true)
            //    PaginationControls.Visibility = Visibility.Visible;
            //else
            //    PaginationControls.Visibility = Visibility.Hidden;

            //if (chkPaginated.IsChecked.HasValue && chkPaginated.IsChecked.Value)
            //{

            //}
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

            SearchChar = cbxSearchChar.SelectedIndex >= 0 ? cbxSearchChar.SelectedValue.ToString() : cbxSearchChar.Text;
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

            string finalResult = string.Empty;
            string spaceText = string.Empty;
            if (addSpaece.HasValue && addSpaece.Value)
                spaceText = " ";
            _results = outputStrings.Select(formattedText => string.Format("{0}{1}{2}", formattedText, splitChar, spaceText));

            if (chkPaginated.IsChecked.HasValue && chkPaginated.IsChecked.Value)
            {
                var currentPageOutput = _results.Skip(Start).Take(_itemCount);
            }
            else
            {
                finalResult = string.Join(string.Empty, _results);
                TxtOutput.Text = finalResult.Remove(finalResult.LastIndexOf(splitChar));
            }
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

        private void cbxPageSize_Selected(object sender, RoutedEventArgs e)
        {
            //_itemCount = (int) cbxPageSize.SelectedValue;
            //_totalPages = TotalItems / _itemCount + (TotalItems % _itemCount);
        }

        private void NxtButton_click(object sender, RoutedEventArgs e)
        {
            //if (Start <= End)
            //    Start += 1;
        }

        private void TxtSource_TextChanged(object sender, TextChangedEventArgs e) {
        }
    }
}
