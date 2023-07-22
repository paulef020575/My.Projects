namespace My.Projects.MyEventArgs
{
    public class MessageEventArgs : System.EventArgs
    {
        public string Message { get; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
