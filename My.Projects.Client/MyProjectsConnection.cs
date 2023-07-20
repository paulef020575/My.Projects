using EPV.Database;

namespace My.Projects.Data
{
    public class MyProjectsConnection : FirebirdConnection, IMyConnector
    {
        public MyProjectsConnection(string connectionString) : base(connectionString)
        {
        }

        public MyProjectsConnection(string server, string database, string userId = "SYSDBA", string password = "masterkey") : base(server, database, userId, password)
        {
        }
    }
}
