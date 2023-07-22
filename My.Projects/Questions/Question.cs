using System;
using My.Projects.ViewModels.Base;

namespace My.Projects.Questions
{
    public class Question
    {
        public string Message { get; }

        public string YesText { get; }

        public string NoText { get; }

        private Action _yesAnswer;

        public Action _noAnswer;

        public Question(string message, Action yesAnswer, Action noAnswer = null,
            string yesText = "ДА", string noText = "НЕТ")
        {
            Message = message;
            _yesAnswer = yesAnswer;
            _noAnswer = noAnswer;
            YesText = yesText;
            NoText = noText;
        }

        #region Commands

        #region YesCommand

        private RelayCommand _yesCommand;

        public RelayCommand YesCommand
        {
            get
            {
                if (_yesCommand == null)
                    _yesCommand = new RelayCommand(x => _yesAnswer.Invoke());
                return _yesCommand;
            }
        }

        #endregion

        #region NoCommand

        private RelayCommand _noCommand;

        public RelayCommand NoCommand
        {
            get
            {
                if (_noCommand == null)
                    _noCommand = new RelayCommand(x => _noAnswer?.Invoke());
                return _noCommand;
            }
        }

        #endregion


        #endregion
    }
}
