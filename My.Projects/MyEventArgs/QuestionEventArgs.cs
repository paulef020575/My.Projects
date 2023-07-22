using System;

namespace My.Projects.MyEventArgs
{
    public class QuestionEventArgs : MessageEventArgs
    {
        public string YesText { get; }

        public string NoText { get; }

        public Action OnYesAnswer { get; }

        public Action OnNoAnswer { get; }
        public QuestionEventArgs(string question, 
            Action onYesAnswer, Action onNoAnswer = null,
            string yesText = "ДА", string noText = "НЕТ")
            : base(question)
        {
            YesText = yesText;
            NoText = noText;

            OnYesAnswer = onYesAnswer;
            OnNoAnswer = onNoAnswer;
        }
    }
}
