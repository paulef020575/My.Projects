using EPV.Database;

namespace My.Projects.Data
{
    public class MyProjectsApi : ApiConnector, IMyConnector
    {
        public MyProjectsApi(string baseAddress) : base(baseAddress)
        {
        }
    }
}
