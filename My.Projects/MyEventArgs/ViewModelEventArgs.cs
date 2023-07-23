using System;
using My.Projects.ViewModels.Base;

namespace My.Projects.MyEventArgs
{
    public class ViewModelEventArgs : EventArgs
    {
        public ViewModel ViewModel { get; }

        public bool RefreshData { get; }

        public ViewModelEventArgs(ViewModel viewModel, bool refreshData = true)
        {
            ViewModel = viewModel;
            RefreshData = refreshData;
        }
    }
}
