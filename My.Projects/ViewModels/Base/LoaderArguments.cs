using EPV.Database;

namespace My.Projects.ViewModels.Base
{
    public class LoaderArguments
    {
        public IConnector Connector { get; private set; }

        public LoaderArguments(IConnector connector)
        {
            Connector = connector;
        }
    }
}
