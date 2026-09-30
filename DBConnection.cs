using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Gr8FoodSystem
{
    public class DBConnection
    {
        private string connStr = @"Data Source=localhost;Initial Catalog=ResturantDB;Integrated Security=True;TrustServerCertificate=True";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connStr);
        }
    }
}