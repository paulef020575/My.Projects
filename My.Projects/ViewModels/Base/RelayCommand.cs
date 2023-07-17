using System;
using System.Windows.Input;

namespace My.Projects.ViewModels.Base
{
    /// <summary>
    ///     Класс "Команда"
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        #region _execute

        /// <summary>
        ///     Действие для выполнения
        /// </summary>
        private Action<object> _execute;

        #endregion

        #region _canExecute

        /// <summary>
        ///     Предикат для определения возможности выполнения
        /// </summary>
        private Predicate<object> _canExecute;

        #endregion

        #endregion

        #region Constructors

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, param => true) { }

        private RelayCommand() { }

        #endregion

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
        
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        
    }
}
