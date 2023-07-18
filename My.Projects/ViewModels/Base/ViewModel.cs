using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using My.Projects.Annotations;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     базовый класс для модели представления
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        #region Properties

        #region Title

        public abstract string Title { get; }

        #endregion

        #region PreviousViewModel

        public virtual ViewModel PreviousViewModel { get; set; }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        private PropertyChangedEventHandler _onPropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => _onPropertyChanged += value;
            remove => _onPropertyChanged -= value;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _onPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Methods

        #region LoadData

        public virtual void LoadData() { }

        #endregion

        #endregion

        #region Events

        #region OnStartProgress

        protected EventHandler<string> startProgress;

        public event EventHandler<string> OnStartProgress
        {
            add { startProgress += value; }
            remove { startProgress -= value; }
        }

        #endregion

        #region OnFinishProgress

        protected EventHandler<string> finishProgress;

        public event EventHandler<string> OnFinishProgress
        {
            add { finishProgress += value; }
            remove { finishProgress -= value; }
        }

        #endregion

        #region OnError

        protected EventHandler<string> _onError;

        public event EventHandler<string> OnError
        {
            add { _onError += value; }
            remove { _onError -= value; }
        }

        #endregion

        #region OnStatusMessage

        protected EventHandler<string> _onStatusMessage;

        public event EventHandler<string> OnStatusMessage
        { 
            add { _onStatusMessage += value; }
            remove { _onStatusMessage -= value; }
        }


        #endregion

        #region OnSwitchToViewModel

        protected EventHandler<ViewModel> _onSwitchToViewModel;

        public event EventHandler<ViewModel> OnSwitchToViewModel
        {
            add { _onSwitchToViewModel += value; }
            remove { _onSwitchToViewModel -= value; }
        }

        #endregion

        #endregion
    }
}
