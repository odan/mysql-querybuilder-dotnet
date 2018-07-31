using System;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlQueryBuilder;

namespace MySqlQueryBuilderTest
{
    public class BaseTest
    {
        private string Dsn = "Database=prisma;Data Source=localhost;User Id=root;Password=;SslMode=none";

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

        protected void CreateMetaTable()
        {
            this.ExecuteSql("DROP TABLE IF EXISTS `meta`;");
            string sql = "CREATE TABLE `meta` ( " +
                         "`id` int(11) NOT NULL AUTO_INCREMENT, " +
                         "`meta_key` varchar(255) NOT NULL, " +
                         "`meta_value` varchar(8000) DEFAULT NULL, " +
                         "PRIMARY KEY(`id`)," +
                         "KEY `meta_key` (`meta_key`) " +
                         ") ENGINE = MEMORY DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci;";
            this.ExecuteSql(sql);
        }

        protected void CreateUserTable()
        {
            this.ExecuteSql("DROP TABLE IF EXISTS `user`;");
            string sql = "CREATE TABLE `user` ( " +
                         "`id` int(11) NOT NULL AUTO_INCREMENT, " +
                         "`name` varchar(255) NOT NULL, " +
                         "`username` varchar(255) NOT NULL, " +
                         "`email` varchar(255) NOT NULL, " +
                         "PRIMARY KEY(`id`)" +
                         ") ENGINE = MEMORY DEFAULT CHARSET = utf8 COLLATE = utf8_unicode_ci;";
            this.ExecuteSql(sql);
        }

        protected void FillUserTable()
        {
            string sql =
                "INSERT INTO `user` (`name`, `username`, `email`) VALUES('myname', 'myusername', 'user@example.com');";
            this.ExecuteSql(sql);
            sql =
                "INSERT INTO `user` (`name`, `username`, `email`) VALUES('myname2', 'myusername2', 'user2@example.com');";
            this.ExecuteSql(sql);
            sql =
                "INSERT INTO `user` (`name`, `username`, `email`) VALUES('myname3', 'myusername3', 'user3@example.com');";
            this.ExecuteSql(sql);
        }
    }
}