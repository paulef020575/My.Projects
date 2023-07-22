using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using My.Projects.Annotations;
using My.Projects.MyEventArgs;

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

        protected EventHandler<MessageEventArgs> _onStartProgress;

        public event EventHandler<MessageEventArgs> OnStartProgress
        {
            add { _onStartProgress += value; }
            remove { _onStartProgress -= value; }
        }

        #endregion

        #region OnFinishProgress

        protected EventHandler<MessageEventArgs> _onFinishProgress;

        public event EventHandler<MessageEventArgs> OnFinishProgress
        {
            add { _onFinishProgress += value; }
            remove { _onFinishProgress -= value; }
        }

        #endregion

        #region OnError

        protected EventHandler<MessageEventArgs> _onError;

        public event EventHandler<MessageEventArgs> OnError
        {
            add { _onError += value; }
            remove { _onError -= value; }
        }

        #endregion

        #region OnStatusMessage

        protected EventHandler<MessageEventArgs> _onStatusMessage;

        public event EventHandler<MessageEventArgs> OnStatusMessage
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

        #region OnQuestion

        protected EventHandler<QuestionEventArgs> _onQuestion;

        public event EventHandler<QuestionEventArgs> OnQuestion
        {
            add => _onQuestion += value;
            remove => _onQuestion -= value;
        }

        #endregion

        #endregion
    }
}
