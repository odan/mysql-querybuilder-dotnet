using System;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlQueryBuilder;

namespace MySqlQueryBuilderTest
{
    public class BaseTest
    {
        private string Dsn = "Database=mysql_query_test;Data Source=localhost;User Id=root;Password=;SslMode=none";

        protected MySqlConnection Connection;

        protected MySqlConnection InitConnection()
        {
            if (Connection != null)
            {
                return Connection;
            }

            Connection = new MySqlConnection(this.Dsn);

            try
            {
                Connection.Open();

                this.CreateUserTable();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }

            return Connection;
        }

        protected Query NewQuery()
        {
            this.InitConnection();
            return new Query(this.Connection);
        }

        protected int ExecuteSql(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            return cmd.ExecuteNonQuery();
        }

        protected MySqlDataReader ExecuteQuery(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            return cmd.ExecuteReader();
        }

        protected void CreateUserTable()
        {
            this.ExecuteSql("DROP TABLE IF EXISTS `users`;");
            string sql = "CREATE TABLE `users` ( " +
                         "`id` int(11) NOT NULL AUTO_INCREMENT, " +
                         "`first_name` varchar(255) NOT NULL, " +
                         "`last_name` varchar(255) NOT NULL, " +
                         "`username` varchar(255) NOT NULL, " +
                         "`email` varchar(255) NOT NULL, " +
                         "PRIMARY KEY(`id`)" +
                         ") ENGINE = MEMORY DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci;";
            this.ExecuteSql(sql);
        }

        protected void FillUserTable()
        {
            string sql =
                "INSERT INTO `users` (`first_name`, `username`, `email`) VALUES('myname', 'myusername', 'user@example.com');";
            this.ExecuteSql(sql);
            sql =
                "INSERT INTO `users` (`first_name`, `username`, `email`) VALUES('myname2', 'myusername2', 'user2@example.com');";
            this.ExecuteSql(sql);
            sql =
                "INSERT INTO `users` (`first_name`, `username`, `email`) VALUES('myname3', 'myusername3', 'user3@example.com');";
            this.ExecuteSql(sql);
        }
    }
}