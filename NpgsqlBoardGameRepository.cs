using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAppPostgresql
{
    public class NpgsqlBoardGameRepository
    {
        public NpgsqlConnection connection; //koneksi database
            private const string CONNECTION_STRING = "Host=localhost:5432;" +
                                                    "Username=postgres;" +
                                                    "Password=123456;" +
                                                    "Database=db_siswa";

        public NpgsqlBoardGameRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }
    }
}
