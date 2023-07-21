using EPV.Data.DataGetters;
using EPV.Database;

namespace My.Projects.Data
{
    public class MyProjectsApi : ApiLink, IMyDataLink
    {
        public MyProjectsApi(string baseAddress) : base(baseAddress)
        {
        }
    }
}
