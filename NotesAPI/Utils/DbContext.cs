using Microsoft.Data.SqlClient;

namespace NotesAPI.Utils
{
    public class DbContext
    {
        private readonly IConfiguration configuration;

        public DbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SqlConnection Connection { 
            get =>  new SqlConnection(configuration.GetConnectionString("SqlDbConnection"));
        }
    }
}
