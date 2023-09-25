using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace Helper_UI.SplitMerge
{
    public class SplitMergeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand firstCmd;
        private ICommand prevCmd;
        private ICommand nextCmd;
        private ICommand LastCmd;

        #region Commands
        public ICommand FirstPageCommand
        {
            get
            {
                if (firstCmd == null)
                {
                //    firstCmd = new RelayCommand(
                //    param =>
                //    {
                //        start = 0;
                //        RefreshProducts();

                //    },
                //    param =>
                //    {
                //        return start - itemCount >= 0 ? true : false;
                //    }
                //);
                //firstCmd = new RelayCommand(param =>)
                }
                return firstCmd;
            }
        }
        #endregion
    }
}
