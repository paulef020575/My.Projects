
using System;
using System.Windows;
using System.Windows.Threading;
using My.Projects.Data;
using My.Projects.ViewModels.Base;
using My.Projects.ViewModels.References;

namespace My.Projects.ViewModels
{
    /// <summary>
    ///     Класс "Модель представления главного окна"
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        #region Properties

        #region Title

        private string _title = "desktop client";
        /// <summary>
        ///     Заголовок окна
        /// </summary>
        public override string Title
        {
            get
            {
                string result = _title;
                if (CurrentViewModel != null && !string.IsNullOrEmpty(CurrentViewModel.Title))
                        result = $"{CurrentViewModel.Title} - {result}";

                return result;
            }
        }

        #endregion

        #region ApiConnector

        /// <summary>
        ///     объект коннектора данных
        /// </summary>
        private MyProjectsApi ApiConnector { get; }

        #endregion

        #region ErrorMessage

        private string _errorMessage;
        /// <summary>
        ///     Сообщение об ошибке
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (!string.Equals(_errorMessage, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasErrorMessage));
                }
            }
        }

        #endregion

        #region CurrentViewModel

        private ViewModel _currentViewModel;
        /// <summary>
        ///     Текущая модель представления
        /// </summary>
        public ViewModel CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        #endregion

        #region HasErrorMessage

        /// <summary>
        ///     Признак "Имеется сообщение об ошибке"
        /// </summary>
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        #endregion

        #region InProgress

        private bool _inProgress;
        /// <summary>
        ///     Признак "Выполняется длительная операция"
        /// </summary>
        public bool InProgress
        {
            get => _inProgress;
            set
            {
                if (_inProgress != value)
                {
                    _inProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region ProgramStatus

        private string _programStatus;
        /// <summary>
        ///     Строка статуса
        /// </summary>
        public string ProgramStatus
        {
            get => _programStatus;
            set
            {
                if (!string.Equals(_programStatus, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    _programStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            ApiConnector = new MyProjectsApi((string)Application.Current.Resources["ApiUri"]);
            InProgress = false;
            ProgramStatus = "Everything is OK";
            //DispatcherTimer timer = new DispatcherTimer{ Interval = new TimeSpan(0, 0, 5)};
            //timer.Tick += Timer_Tick;
            //    timer.Start();

            //ShowViewModel(new DepartmentListViewModel(ApiConnector));
        }

        #endregion

        #region Methods

        #region StartProgress

        /// <summary>
        ///     Отображает запуск долгого процесса
        /// </summary>
        /// <param name="sender">класс, запустивший</param>
        /// <param name="e">строка статуса</param>
        private void StartProgress(object sender, string e)
        {
            ProgramStatus = e;
            InProgress = true;
        }

        #endregion

        #region FinishProgress

        private void FinishProgress(object sender, string e = "")
        {
            ProgramStatus = e;
            InProgress = false;
        }

        #endregion

        #region ShowViewModel

        public void ShowViewModel(ViewModel viewModel)
        {
            if (CurrentViewModel != null)
            {
                CurrentViewModel.OnStartProgress -= StartProgress;
                CurrentViewModel.OnFinishProgress -= FinishProgress;
                CurrentViewModel.OnError -= ShowError;
                CurrentViewModel.OnStatusMessage -= ShowStatusMessage;
                CurrentViewModel.OnSwitchToViewModel -= SwitchToViewModel;
            }

            viewModel.PreviousViewModel = CurrentViewModel;

            viewModel.OnStartProgress += StartProgress;
            viewModel.OnFinishProgress += FinishProgress;
            viewModel.OnError += ShowError;
            viewModel.OnStatusMessage += ShowStatusMessage;
            viewModel.OnSwitchToViewModel += SwitchToViewModel;

            viewModel.LoadData();
            CurrentViewModel = viewModel;
        }

        #endregion

        #region SwitchToViewModel

        private void SwitchToViewModel(object sender, ViewModel viewModel)
        {
            ShowViewModel(viewModel);
        }

        #endregion

        #region ShowStartViewModel

        internal void ShowStartViewModel()
        {
            ShowViewModel(new DepartmentListViewModel(ApiConnector));
        }


        #endregion

        #region CloseApp

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ShowError

        private void ShowError(object sender, string e)
        {
            ErrorMessage = e;
            DispatcherTimer errorTimer = new DispatcherTimer();
            errorTimer.Interval = new TimeSpan(0, 0, 5);
            errorTimer.Tick += ErrorTimer_Tick;
            errorTimer.Start();
        }

        private void ErrorTimer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer) sender).Stop();
            ErrorMessage = "";
        }

        #endregion

        #region ShowStatusMessage

        private void ShowStatusMessage(object sender, string e)
        {
            ProgramStatus = e;
        }


        #endregion

        #endregion

        #region Commands

        #region CloseCommand

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(x => CloseApp());

                return _closeCommand;
            }
        }


        #endregion

        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            InProgress = !InProgress;
        }
    }
}
