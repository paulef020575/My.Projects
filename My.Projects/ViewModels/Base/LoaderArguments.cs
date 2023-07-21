using EPV.Database;
using My.Projects.Data;

namespace My.Projects.ViewModels.Base
{
    public class LoaderArguments
    {
        public IMyDataLink Connector { get; private set; }

        public LoaderArguments(IMyDataLink connector)
        {
            Connector = connector;
        }
    }
}
