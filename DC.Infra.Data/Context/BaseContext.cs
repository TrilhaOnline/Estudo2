using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DC.Infra.Data.Context
{
    public sealed class BaseContext
    {
        public static IDbConnection Conn() => new SqlConnection(ConfigurationManager.ConnectionStrings["dcconnect"].ConnectionString);
    }
}
