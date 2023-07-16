using EPV.Database;

namespace My.Projects.Web.Classes
{
    public class MyProjectsConnection : FirebirdConnection
    {
        public MyProjectsConnection(string connectionString) : base(connectionString)
        {
        }

        public MyProjectsConnection(string server, string database, string userId = "SYSDBA", string password = "masterkey") : base(server, database, userId, password)
        {
        }
    }
}
