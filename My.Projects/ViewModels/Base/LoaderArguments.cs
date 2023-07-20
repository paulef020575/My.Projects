using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    public class LoaderArguments
    {
        public IDataConnection Connector { get; private set; }

        public LoaderArguments(IDataConnection connector)
        {
            Connector = connector;
        }
    }
}
