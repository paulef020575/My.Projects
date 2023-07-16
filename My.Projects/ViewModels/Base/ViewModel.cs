﻿using System.ComponentModel;
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

    }
}
