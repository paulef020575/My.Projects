﻿
using System;
using System.Windows;
using System.Windows.Threading;
using EPV.Data.DataGetters;
using EPV.Database;
using My.Projects.Data;
using My.Projects.MyEventArgs;
using My.Projects.Questions;
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
        private IMyDataLink ApiConnector => DataChannels.DataLink as IMyDataLink;

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

        #region Question

        private Question _question;

        public Question Question
        {
            get => _question;
            set
            {
                if (_question != value)
                {
                    _question = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasQuestion));
                }
            }
        }

        public bool HasQuestion => (Question != null);

        #endregion

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            DataChannels.DataLink = new MyProjectsApi((string)Application.Current.Resources["ApiUri"]);
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
        private void StartProgress(object sender, MessageEventArgs e)
        {
            ProgramStatus = e.Message;
            InProgress = true;
        }

        #endregion

        #region FinishProgress

        private void FinishProgress(object sender, MessageEventArgs e)
        {
            ProgramStatus = e.Message;
            InProgress = false;
        }

        #endregion

        #region ShowViewModel

        public void ShowViewModel(ViewModel viewModel, bool refreshData = true)
        {
            if (CurrentViewModel != null)
            {
                CurrentViewModel.OnStartProgress -= StartProgress;
                CurrentViewModel.OnFinishProgress -= FinishProgress;
                CurrentViewModel.OnError -= ShowError;
                CurrentViewModel.OnStatusMessage -= ShowStatusMessage;
                CurrentViewModel.OnSwitchToViewModel -= SwitchToViewModel;
                CurrentViewModel.OnQuestion -= ShowQuestion;
            }

            viewModel.PreviousViewModel = CurrentViewModel;

            viewModel.OnStartProgress += StartProgress;
            viewModel.OnFinishProgress += FinishProgress;
            viewModel.OnError += ShowError;
            viewModel.OnStatusMessage += ShowStatusMessage;
            viewModel.OnSwitchToViewModel += SwitchToViewModel;
            viewModel.OnQuestion += ShowQuestion;

            if (refreshData)
                viewModel.LoadData();
            CurrentViewModel = viewModel;
        }

        #endregion

        #region SwitchToViewModel

        private void SwitchToViewModel(object sender, ViewModelEventArgs e)
        {
            ShowViewModel(e.ViewModel, e.RefreshData);
        }

        #endregion

        #region ShowStartViewModel

        internal void ShowStartViewModel()
        {
            ShowViewModel(new DepartmentListViewModel());
        }


        #endregion

        #region CloseApp

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ShowError

        private void ShowError(object sender, MessageEventArgs e)
        {
            ErrorMessage = e.Message;
            DispatcherTimer errorTimer = new DispatcherTimer();
            errorTimer.Interval = new TimeSpan(0, 0, 5);
            errorTimer.Tick += ErrorTimer_Tick;
            errorTimer.Start();
        }

        private void ErrorTimer_Tick(object sender, System.EventArgs e)
        {
            ((DispatcherTimer) sender).Stop();
            ErrorMessage = "";
        }

        #endregion

        #region ShowStatusMessage

        private void ShowStatusMessage(object sender, MessageEventArgs e)
        {
            ProgramStatus = e.Message;
        }


        #endregion

        #region ShowQuestion

        private void ShowQuestion(object sender, QuestionEventArgs e)
        {
            Question = new Question(
                e.Message,
                () => 
                { e.OnYesAnswer.Invoke();
                    ClearQuestion();
                },
                () => { e.OnNoAnswer?.Invoke();
                    ClearQuestion();
                },
                e.YesText,
                e.NoText);
        }

        private void ClearQuestion()
        {
            Question = null;
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
